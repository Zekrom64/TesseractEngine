
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.Core.Graphics.Model {

	public class OBJModelFormat : IModelFormat {

		public bool CanLoad => true;

		public bool CanSave => false;

		private class OBJModelParser {

			private const int ParallelThreshold = 1000;

			// The list of face indices
			private readonly List<(int,int,int)> indices = new();
			// The lists of vertices, texture coordinates, and normals
			private readonly List<Vector3> vertices = new();
			private readonly List<Vector2> texCoords = new();
			private readonly List<Vector3> normals = new();

			// If the face indices are "paired" (all indices for each vertex are equal)
			private bool isPairedIndices = true;

			private Dictionary<string, List<ModelNode>> objects = new();

			// Object tracking state
			private string currentObjectName = "";
			private List<ModelNode> currentObjectGroups = new();

			// Group tracking state
			private string currentGroupName = "";
			private ModelDrawCall currentGroupDraw = new() { Mode = DrawMode.TriangleList };

			private void CheckDrawCall() {
				if (currentGroupDraw.Length > 0) {
					currentObjectGroups.Add(new ModelNode() {
						Name = currentGroupName,
						DrawCalls = new ModelDrawCall[] { currentGroupDraw },
						LocalTransform = Matrix4x4.Identity
					});
				}
			}

			private void CheckGroup() {
				if (currentObjectGroups.Count > 0) {
					objects[currentObjectName] = currentObjectGroups;
					currentObjectGroups = new();
				}
			}

			public OBJModel ToModel() {
				// Check any trailing vertex groupings
				CheckDrawCall();
				CheckGroup();

				if (isPairedIndices) {
					int[] mindices = new int[indices.Count];
					if (indices.Count > ParallelThreshold) Parallel.For(0, indices.Count, i => mindices[i] = indices[i].Item1);
					else for (int i = 0; i < indices.Count; i++) mindices[i] = indices[i].Item1;

					return new OBJModel() {
						IndexPlane = mindices,
						VertexPlane = vertices.ToArray(),
						TexCoordPlane = texCoords.Count > 0 ? texCoords.ToArray() : Array.Empty<Vector2>(),
						NormalPlane = normals.Count > 0 ? normals.ToArray() : Array.Empty<Vector3>()
					};
				} else {
					int[] mindices = new int[indices.Count];
					Vector3[] mvertices = new Vector3[indices.Count];
					Vector2[]? mtexCoords = texCoords.Count > 0 ? new Vector2[indices.Count] : null;
					Vector3[]? mnormals = normals.Count > 0 ? new Vector3[indices.Count] : null;
					if (indices.Count > ParallelThreshold) {
						Parallel.For(0, indices.Count, i => {
							mindices[i] = i;
							var index = indices[i];
							mvertices[i] = vertices[index.Item1];
							if (mtexCoords != null) mtexCoords[i] = texCoords[index.Item2];
							if (mnormals != null) mnormals[i] = normals[index.Item3];
						});
					} else {
						for(int i = 0; i < indices.Count; i++) {
							mindices[i] = i;
							var index = indices[i];
							mvertices[i] = vertices[index.Item1];
							if (mtexCoords != null) mtexCoords[i] = texCoords[index.Item2];
							if (mnormals != null) mnormals[i] = normals[index.Item3];
						}
					}
					return new OBJModel() {
						IndexPlane = mindices,
						VertexPlane = mvertices,
						TexCoordPlane = mtexCoords,
						NormalPlane = mnormals
					};
				}
			}

			public void ParseLine(string line) {
				string NextToken() {
					line = line.TrimStart();
					int off = 0;
					for (; off < line.Length; off++)
						if (char.IsWhiteSpace(line[off])) break;
					string token = line[0..off];
					line = line[off..];
					return token;
				}
				float NextFloat() => float.Parse(NextToken());
				(int,int,int) NextIndex() {
					(int, int, int) ival = (-1,-1,-1);
					string idx = NextToken();
					string[] parts = idx.Split('/');
					ival.Item1 = int.Parse(parts[0]) - 1;
					if (parts.Length > 1) {
						ival.Item2 = int.Parse(parts[1]) - 1;
						if (ival.Item2 != ival.Item1) isPairedIndices = false;
					}
					if (parts.Length > 2) {
						ival.Item3 = int.Parse(parts[2]) - 1;
						if (ival.Item3 != ival.Item1 || ival.Item3 != ival.Item2) isPairedIndices = false;
					}
					return ival;
				}

				string tok = NextToken();
				if (tok.StartsWith('#')) return;
				switch(tok) {
					case "v": // Vertex (position)
						vertices.Add(new Vector3(NextFloat(), NextFloat(), NextFloat()));
						break;
					case "vt": // Texture coordinate
						texCoords.Add(new Vector2(NextFloat(), NextFloat()));
						break;
					case "vn": // Normal
						normals.Add(new Vector3(NextFloat(), NextFloat(), NextFloat()));
						break;
					case "f": // Face (triangle)
						indices.Add(NextIndex());
						indices.Add(NextIndex());
						indices.Add(NextIndex());
						currentGroupDraw.Length += 3;
						break;
					case "o": // (Named) Object
						CheckDrawCall();
						CheckGroup();
						currentObjectName = line.Trim();
						currentObjectGroups = new List<ModelNode>();
						break;
					case "g": // (Named) Vertex Group
						CheckDrawCall();
						currentGroupName = line.Trim();
						currentGroupDraw = new ModelDrawCall() {
							Mode = DrawMode.TriangleList,
							Offset = currentGroupDraw.Offset + currentGroupDraw.Length
						};
						break;
				}
			}

		}

		public IModel Load(Stream stream) {
			StreamReader sr = new(stream);
			OBJModelParser parser = new();
			string? line;
			while((line = sr.ReadLine()) != null) parser.ParseLine(line);
			return parser.ToModel();
		}

		public void Save(IModel model, Stream stream) => throw new NotImplementedException();

		public class OBJModel : IModel {

			public int[]? IndexPlane { get; init; }

			public Vector3[]? VertexPlane { get; init; }

			public Vector2[]? TexCoordPlane { get; init; }

			public Vector3[]? NormalPlane { get; init; }

			public ModelNode RootNode { get; init; }

		}

	}

}
