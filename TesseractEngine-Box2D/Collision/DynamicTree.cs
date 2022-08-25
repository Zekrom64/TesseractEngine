using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.Box2D.NET {
	
	public struct TreeNode<T> {

		/// <summary>
		/// If the node is a leaf (ie. has no children).
		/// </summary>
		public bool IsLeaf => Child1 == Box2D.NullNode;

		/// <summary>
		/// The bounding box encompassing this node and its children.
		/// </summary>
		public AABB AABB;

		/// <summary>
		/// The userdata associated with this node.
		/// </summary>
		public T UserData;

		/// <summary>
		/// The index of the parent node if allocated, or the index of the next free node.
		/// </summary>
		public int ParentOrNext;

		/// <summary>
		/// The index of the first child node.
		/// </summary>
		public int Child1;

		/// <summary>
		/// The index of the second child node.
		/// </summary>
		public int Child2;

		/// <summary>
		/// The height of this node in the tree.
		/// </summary>
		public int Height;

		/// <summary>
		/// Flag indicating if the node has been moved in the tree.
		/// </summary>
		public bool Moved;

	}

	public class DynamicTree<T> {

		public DynamicTree() {
			root = Box2D.NullNode;
			nodeCount = 16;
			nodes = new TreeNode<T>[16];
			for (int i = 0; i < nodes.Length - 1; i++) nodes[i] = new TreeNode<T>() { ParentOrNext = i + 1, Height = -1 };
			nodes[^1] = new TreeNode<T>() { ParentOrNext = Box2D.NullNode, Height = -1 };
			freeList = 0;
		}

		public int CreateProxy(AABB aabb, T userData) {
			int proxyId = AllocateNode();

			Vector2 r = new(Box2D.AABBExtension);
			ref TreeNode<T> node = ref nodes[proxyId];
			node.AABB = new AABB() { LowerBound = aabb.LowerBound - r, UpperBound = aabb.UpperBound + r };
			node.UserData = userData;
			node.Height = 0;
			node.Moved = true;
			InsertLeaf(proxyId);
			return proxyId;
		}

		public void DestroyProxy(int proxyId) {
			RemoveLeaf(proxyId);
			FreeNode(proxyId);
		}

		public bool MoveProxy(int proxyId, AABB aabb, Vector2 displacement) {
			Vector2 r = new(Box2D.AABBExtension);
			AABB fatAABB = new() { LowerBound = aabb.LowerBound - r, UpperBound = aabb.UpperBound + r };

			Vector2 d = Box2D.AABBMultiplier * displacement;

			if (d.X < 0) fatAABB.LowerBound.X += d.X;
			else fatAABB.UpperBound.X += d.X;
			if (d.Y < 0) fatAABB.LowerBound.Y += d.Y;
			else fatAABB.UpperBound.Y += d.Y;

			AABB treeAABB = nodes[proxyId].AABB;
			if (treeAABB.Contains(aabb)) {
				AABB hugeAABB = new() { LowerBound = fatAABB.LowerBound - 4 * r, UpperBound = fatAABB.UpperBound + 4 * r };
				if (hugeAABB.Contains(treeAABB)) return false;
			}

			RemoveLeaf(proxyId);
			nodes[proxyId].AABB = fatAABB;
			InsertLeaf(proxyId);
			nodes[proxyId].Moved = true;
			return true;
		}

		public T GetUserData(int proxyId) => nodes[proxyId].UserData;

		public bool WasMoved(int proxyId) => nodes[proxyId].Moved;

		public void ClearMoved(int proxyId) => nodes[proxyId].Moved = false;

		public AABB GetFatAABB(int proxyId) => nodes[proxyId].AABB;

		public void Query(Func<int,bool> callback, AABB aabb) {
			Stack<int> stack = new(256);
			stack.Push(root);

			while(stack.Count > 0) {
				int nodeId = stack.Pop();
				if (nodeId == Box2D.NullNode) continue;

				ref TreeNode<T> node = ref nodes[nodeId];
				if (Box2D.TestOverlap(node.AABB, aabb)) {
					if (node.IsLeaf) {
						bool processed = callback(nodeId);
						if (!processed) return;
					} else {
						stack.Push(node.Child1);
						stack.Push(node.Child2);
					}
				}
			}
		}

		public void RayCast(Func<RayCastInput, int, float> callback, in RayCastInput input) {
			Vector2 p1 = input.P1;
			Vector2 p2 = input.P2;
			Vector2 r = p2 - p1;
			if (r.LengthSquared() == 0) throw new ArgumentException("Cannot cast ray of zero length", nameof(input));
			r = r.Normalize();

			Vector2 v = r.CrossT(1);
			Vector2 abs_v = v.Abs();

			float maxFraction = input.MaxFraction;
			
			AABB segmentAABB;
			{
				Vector2 t = p1 + maxFraction * (p2 - p1);
				segmentAABB = new() {
					LowerBound = p1.Min(t),
					UpperBound = p1.Max(t)
				};
			}

			Stack<int> stack = new(256);
			stack.Push(root);

			while(stack.Count > 0) {
				int nodeId = stack.Pop();
				if (nodeId == Box2D.NullNode) continue;

				ref TreeNode<T> node = ref nodes[nodeId];
				if (!Box2D.TestOverlap(node.AABB, segmentAABB)) continue;

				Vector2 c = node.AABB.Center;
				Vector2 h = node.AABB.Extents;
				float separation = Math.Abs(v.Dot(p1 - c)) - abs_v.Dot(h);
				if (separation > 0) continue;

				if (node.IsLeaf) {
					RayCastInput subInput = new() {
						P1 = input.P1,
						P2 = input.P2,
						MaxFraction = maxFraction
					};

					float value = callback(subInput, nodeId);
					if (value == 0) return;
					if (value > 0) {
						maxFraction = value;
						Vector2 t = p1 + maxFraction * (p2 - p1);
						segmentAABB.LowerBound = p1.Min(t);
						segmentAABB.UpperBound = p1.Max(t);
					}
				} else {
					stack.Push(node.Child1);
					stack.Push(node.Child2);
				}
			}
		}

		public void Validate() {
			ValidateStructure(root);
			ValidateMetrics(root);

			int freeCount = 0;
			int freeIndex = freeList;
			while(freeIndex != Box2D.NullNode) {
				Debug.Assert(0 <= freeIndex && freeIndex < nodes.Length);
				freeIndex = nodes[freeIndex].ParentOrNext;
				freeCount++;
			}
			Debug.Assert(Height == ComputeHeight());
			Debug.Assert(nodeCount + freeCount == nodes.Length);
		}

		public int Height => root == Box2D.NullNode ? 0 : nodes[root].Height;

		public int MaxBalance {
			get {
				int maxBalance = 0;
				for(int i = 0; i < nodes.Length; i++) {
					TreeNode<T> node = nodes[i];
					if (node.Height <= 1) continue;

					int child1 = node.Child1;
					int child2 = node.Child2;
					int balance = Math.Abs(nodes[child2].Height - nodes[child1].Height);
					maxBalance = Math.Max(maxBalance, balance);
				}
				return maxBalance;
			}
		}

		public float AreaRatio {
			get {
				if (this.root == Box2D.NullNode) return 0;

				TreeNode<T> root = nodes[this.root];
				float rootArea = root.AABB.Perimeter;

				float totalArea = 0;
				for(int i = 0; i < nodes.Length; i++) {
					TreeNode<T> node = nodes[i];
					if (node.Height < 0) continue;
					totalArea += node.AABB.Perimeter;
				}

				return totalArea / rootArea;
			}
		}

		public void RebuildBottomUp() {
			int[] nodes = new int[nodeCount];
			int count = 0;

			for(int i = 0; i < this.nodes.Length; i++) {
				if (this.nodes[i].Height < 0) continue;
				if (this.nodes[i].IsLeaf) {
					this.nodes[i].ParentOrNext = Box2D.NullNode;
					nodes[i] = count++;
				} else FreeNode(i);
			}

			while(count > 1) {
				float minCost = Box2D.MaxFloat;
				int iMin = -1, jMin = -1;
				for(int i = 0; i < count; i++) {
					AABB aabbi = this.nodes[nodes[i]].AABB;
					for(int j = 0; j < count; j++) {
						AABB aabbj = this.nodes[nodes[j]].AABB;
						AABB b = new();
						b.Combine(aabbi, aabbj);
						float cost = b.Perimeter;
						if (cost < minCost) {
							iMin = i;
							jMin = j;
							minCost = cost;
						}
					}
				}

				int index1 = nodes[iMin];
				int index2 = nodes[jMin];
				ref TreeNode<T> child1 = ref this.nodes[index1];
				ref TreeNode<T> child2 = ref this.nodes[index2];
				
				int parentIndex = AllocateNode();
				ref TreeNode<T> parent = ref this.nodes[parentIndex];
				parent.Child1 = index1;
				parent.Child2 = index2;
				parent.Height = 1 + Math.Max(child1.Height, child2.Height);
				parent.AABB.Combine(child1.AABB, child2.AABB);
				parent.ParentOrNext = Box2D.NullNode;

				child1.ParentOrNext = parentIndex;
				child2.ParentOrNext = parentIndex;

				nodes[jMin] = nodes[^1];
				nodes[iMin] = parentIndex;
				count--;
			}

			root = nodes[0];
			Validate();
		}

		public void ShiftOrigin(Vector2 newOrigin) {
			for(int i = 0; i < nodes.Length; i++) {
				ref AABB aabb = ref nodes[i].AABB;
				aabb.LowerBound -= newOrigin;
				aabb.UpperBound -= newOrigin;
			}
		}

		private int AllocateNode() {
			if (freeList == Box2D.NullNode) {
				Array.Resize(ref nodes, nodes.Length * 2);
				for (int i = nodeCount; i < nodes.Length - 1; i++) nodes[i] = new TreeNode<T>() { ParentOrNext = i + 1, Height = -1 };
				nodes[^1] = new TreeNode<T>() { ParentOrNext = Box2D.NullNode, Height = -1 };
				freeList = nodeCount;
			}
			int nodeId = freeList;
			freeList = nodes[nodeId].ParentOrNext;
			ref TreeNode<T> node = ref nodes[nodeId];
			node.ParentOrNext = Box2D.NullNode;
			node.Child1 = Box2D.NullNode;
			node.Child2 = Box2D.NullNode;
			node.Height = 0;
			node.UserData = default!;
			node.Moved = false;
			nodeCount++;
			return nodeId;
		}

		private void FreeNode(int node) {
			nodes[node].ParentOrNext = freeList;
			nodes[node].Height = -1;
			freeList = node;
			nodeCount--;
		}

		private void InsertLeaf(int leaf) {
			if (root == Box2D.NullNode) {
				root = leaf;
				nodes[root].ParentOrNext = Box2D.NullNode;
				return;
			}

			AABB leafAABB = nodes[leaf].AABB;
			int index = root;
			while (!nodes[index].IsLeaf) {
				int child1 = nodes[index].Child1;
				int child2 = nodes[index].Child2;

				float area = nodes[index].AABB.Perimeter;

				AABB combinedAABB = new();
				combinedAABB.Combine(nodes[index].AABB, leafAABB);
				float combinedArea = combinedAABB.Perimeter;

				float cost = 2 * combinedArea;
				float inheritanceCost = 2 * (combinedArea - area);

				float cost1;
				if (nodes[child1].IsLeaf) {
					AABB aabb = new();
					aabb.Combine(leafAABB, nodes[child1].AABB);
					cost1 = aabb.Perimeter + inheritanceCost;
				} else {
					AABB aabb = new();
					aabb.Combine(leafAABB, nodes[child1].AABB);
					float oldArea = nodes[child1].AABB.Perimeter;
					float newArea = aabb.Perimeter;
					cost1 = (newArea - oldArea) + inheritanceCost;
				}

				float cost2;
				if (nodes[child2].IsLeaf) {
					AABB aabb = new();
					aabb.Combine(leafAABB, nodes[child2].AABB);
					cost2 = aabb.Perimeter + inheritanceCost;
				} else {
					AABB aabb = new();
					aabb.Combine(leafAABB, nodes[child2].AABB);
					float oldArea = nodes[child2].AABB.Perimeter;
					float newArea = aabb.Perimeter;
					cost2 = (newArea - oldArea) + inheritanceCost;
				}

				if (cost < cost1 && cost < cost2) break;
				if (cost1 < cost2) index = child1;
				else index = child2;
			}

			int sibling = index;
			int oldParent = nodes[sibling].ParentOrNext;
			int newParent = AllocateNode();
			ref TreeNode<T> parent = ref nodes[newParent];
			parent.ParentOrNext = oldParent;
			parent.UserData = default!;
			parent.AABB.Combine(leafAABB, nodes[sibling].AABB);
			parent.Height = nodes[sibling].Height + 1;

			if (oldParent != Box2D.NullNode) {
				if (nodes[oldParent].Child1 == sibling) nodes[oldParent].Child1 = newParent;
				else nodes[oldParent].Child2 = newParent;
				nodes[newParent].Child1 = sibling;
				nodes[newParent].Child2 = leaf;
				nodes[sibling].ParentOrNext = newParent;
				nodes[leaf].ParentOrNext = newParent;
			} else {
				nodes[newParent].Child1 = sibling;
				nodes[newParent].Child2 = leaf;
				nodes[sibling].ParentOrNext = newParent;
				nodes[leaf].ParentOrNext = newParent;
				root = newParent;
			}

			index = nodes[leaf].ParentOrNext;
			while(index != Box2D.NullNode) {
				index = Balance(index);

				ref TreeNode<T> node = ref nodes[index];
				int child1 = node.Child1;
				int child2 = node.Child2;
				node.Height = 1 + Math.Max(nodes[child1].Height, nodes[child2].Height);
				node.AABB.Combine(nodes[child1].AABB, nodes[child2].AABB);
				index = nodes[index].ParentOrNext;
			}
		}

		private void RemoveLeaf(int leaf) {
			if (leaf == root) {
				root = Box2D.NullNode;
				return;
			}

			int parent = nodes[leaf].ParentOrNext;
			int grandParent = nodes[parent].ParentOrNext;
			int sibling;
			if (nodes[parent].Child1 == leaf) sibling = nodes[parent].Child2;
			else sibling = nodes[parent].Child1;

			if (grandParent != Box2D.NullNode) {
				if (nodes[grandParent].Child1 == parent) nodes[grandParent].Child1 = sibling;
				else nodes[grandParent].Child2 = sibling;
				nodes[sibling].ParentOrNext = grandParent;
				FreeNode(parent);

				int index = grandParent;
				while(index != Box2D.NullNode) {
					index = Balance(index);

					int child1 = nodes[index].Child1;
					int child2 = nodes[index].Child2;

					nodes[index].AABB.Combine(nodes[child1].AABB, nodes[child2].AABB);
					nodes[index].Height = 1 + Math.Max(nodes[child1].Height, nodes[child2].Height);

					index = nodes[index].ParentOrNext;
				}
			} else {
				root = sibling;
				nodes[sibling].ParentOrNext = Box2D.NullNode;
				FreeNode(parent);
			}
		}

		private int Balance(int iA) {
			Debug.Assert(iA != Box2D.NullNode);

			ref TreeNode<T> A = ref nodes[iA];
			if (A.IsLeaf || A.Height < 2) return iA;

			int iB = A.Child1;
			int iC = A.Child2;
			Debug.Assert(0 <= iB && iB < nodes.Length);
			Debug.Assert(0 <= iC && iC < nodes.Length);
			ref TreeNode<T> B = ref nodes[iB];
			ref TreeNode<T> C = ref nodes[iC];

			int balance = C.Height - B.Height;

			if (balance > 1) {
				int iF = C.Child1;
				int iG = C.Child2;
				Debug.Assert(0 <= iF && iF < nodes.Length);
				Debug.Assert(0 <= iG && iG < nodes.Length);
				ref TreeNode<T> F = ref nodes[iF];
				ref TreeNode<T> G = ref nodes[iG];

				C.Child1 = iA;
				C.ParentOrNext = A.ParentOrNext;
				A.ParentOrNext = iC;

				if (C.ParentOrNext != Box2D.NullNode) {
					if (nodes[C.ParentOrNext].Child1 == iA) {
						nodes[C.ParentOrNext].Child1 = iC;
					} else {
						Debug.Assert(nodes[C.ParentOrNext].Child2 == iA);
						nodes[C.ParentOrNext].Child2 = iC;
					}
				} else {
					root = iC;
				}

				if (F.Height > G.Height) {
					C.Child2 = iF;
					A.Child2 = iG;
					G.ParentOrNext = iA;
					A.AABB.Combine(B.AABB, G.AABB);
					C.AABB.Combine(A.AABB, F.AABB);
					A.Height = 1 + Math.Max(B.Height, G.Height);
					C.Height = 1 + Math.Max(A.Height, F.Height);
				} else {
					C.Child2 = iG;
					A.Child2 = iF;
					F.ParentOrNext = iA;
					A.AABB.Combine(B.AABB, F.AABB);
					C.AABB.Combine(A.AABB, G.AABB);
					A.Height = 1 + Math.Max(B.Height, F.Height);
					C.Height = 1 + Math.Max(A.Height, G.Height);
				}

				return iC;
			}

			if (balance < -1) {
				int iD = B.Child1;
				int iE = B.Child2;
				Debug.Assert(0 <= iD && iD <= nodes.Length);
				Debug.Assert(0 <= iE && iE <= nodes.Length);
				ref TreeNode<T> D = ref nodes[iD];
				ref TreeNode<T> E = ref nodes[iE];

				B.Child1 = iA;
				B.ParentOrNext = A.ParentOrNext;
				A.ParentOrNext = iB;

				if (B.ParentOrNext != Box2D.NullNode) {
					if (nodes[B.ParentOrNext].Child1 == iA) {

					} else {
						Debug.Assert(nodes[B.ParentOrNext].Child2 == iA);
						nodes[B.ParentOrNext].Child2 = iB;
					}
				} else {
					root = iB;
				}

				if (D.Height > E.Height) {
					B.Child2 = iD;
					A.Child1 = iE;
					E.ParentOrNext = iA;
					A.AABB.Combine(C.AABB, E.AABB);
					B.AABB.Combine(A.AABB, D.AABB);

					A.Height = 1 + Math.Max(C.Height, E.Height);
					B.Height = 1 + Math.Max(A.Height, D.Height);
				} else {
					B.Child2 = iE;
					A.Child1 = iD;
					D.ParentOrNext = iA;
					A.AABB.Combine(C.AABB, D.AABB);
					B.AABB.Combine(A.AABB, E.AABB);

					A.Height = 1 + Math.Max(C.Height, D.Height);
					B.Height = 1 + Math.Max(A.Height, E.Height);
				}

				return iB;
			}

			return iA;
		}

		private int ComputeHeight() => ComputeHeight(root);

		private int ComputeHeight(int nodeId) {
			ref TreeNode<T> node = ref nodes[nodeId];
			if (node.IsLeaf) return 0;
			int height1 = ComputeHeight(node.Child1);
			int height2 = ComputeHeight(node.Child2);
			return 1 + Math.Max(height1, height2);
		}

		private void ValidateStructure(int index) {
			if (index == Box2D.NullNode) return;
			if (index == root) {
				Debug.Assert(nodes[index].ParentOrNext == Box2D.NullNode);
			}

			TreeNode<T> node = nodes[index];

			int child1 = node.Child1;
			int child2 = node.Child2;

			if (node.IsLeaf) {
				Debug.Assert(child1 == Box2D.NullNode);
				Debug.Assert(child2 == Box2D.NullNode);
				Debug.Assert(node.Height == 0);
				return;
			}

			Debug.Assert(0 <= child1 && child1 < nodes.Length);
			Debug.Assert(0 <= child2 && child2 < nodes.Length);
			Debug.Assert(nodes[child1].ParentOrNext == index);
			Debug.Assert(nodes[child2].ParentOrNext == index);

			ValidateStructure(child1);
			ValidateStructure(child2);
		}

		private void ValidateMetrics(int index) {
			if (index == Box2D.NullNode) return;

			TreeNode<T> node = nodes[index];

			int child1 = node.Child1;
			int child2 = node.Child2;

			if (node.IsLeaf) {
				Debug.Assert(child1 == Box2D.NullNode);
				Debug.Assert(child2 == Box2D.NullNode);
				Debug.Assert(node.Height == 0);
				return;
			}

			Debug.Assert(0 <= child1 && child1 < nodes.Length);
			Debug.Assert(0 <= child2 && child2 < nodes.Length);

			Debug.Assert(Height == 1 + Math.Max(nodes[child1].Height, nodes[child2].Height));

			AABB aabb = new();
			aabb.Combine(nodes[child1].AABB, nodes[child2].AABB);

			Debug.Assert(aabb.LowerBound == node.AABB.LowerBound);
			Debug.Assert(aabb.UpperBound == node.AABB.UpperBound);

			ValidateMetrics(child1);
			ValidateMetrics(child2);
		}

		// The index of the root node
		private int root;
		
		// Array of all nodes in the tree
		private TreeNode<T>[] nodes = Array.Empty<TreeNode<T>>();
		// The number of active nodes in the tree
		private int nodeCount;

		// Index pointing to the first free node in the node array
		private int freeList;

	}

	public static partial class Box2D {

		/// <summary>
		/// Index value used for a null node in a <see cref="DynamicTree{T}"/>.
		/// </summary>
		public const int NullNode = -1;

	}

}
