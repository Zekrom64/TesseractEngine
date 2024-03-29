﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {
	
	public class GLVertexArray : IGLObject, IVertexArray {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public uint ID { get; }

		public VertexFormat Format { get; }

		public GLIndexType? IndexType { get; } = null;

		public nint IndexOffset { get; }

		public GLVertexArray(GLGraphics graphics, VertexArrayCreateInfo createInfo) {
			Graphics = graphics;
			Format = createInfo.Format;

			var dsa = GL.ARBDirectStateAccess;
			var vab = GL.ARBVertexAttribBinding;
			var gl33 = GL.GL33!;

			BufferBinding GetBuffer(uint binding) {
				BufferBinding? bufbinding = null;
				foreach(var vbo in createInfo.VertexBuffers) {
					if (vbo.Index == binding) {
						bufbinding = vbo.Binding;
						break;
					}
				}
				if (bufbinding == null) {
					gl33.DeleteVertexArrays(ID);
					throw new GLException($"Missing buffer binding for vertex binding {binding}");
				}
				return bufbinding.Value;
			}

			if (dsa != null) {
				// Use direct state access if possible
				ID = dsa.CreateVertexArrays();
				foreach(VertexAttrib attrib in Format.Attributes) {
					dsa.EnableVertexArrayAttrib(ID, attrib.Location);
					dsa.VertexArrayAttribBinding(ID, attrib.Location, attrib.Binding);
					var format = GLEnums.StdToGLFormat(attrib.Format);
					if (format == null) throw new ArgumentException($"Cannot convert format {attrib.Format}");
					dsa.VertexArrayAttribFormat(ID, attrib.Location, format.Count, format.Type, attrib.Format.IsNumberFormatNormalized, attrib.Offset);
				}
				foreach(VertexBinding binding in Format.Bindings) {
					BufferBinding bufbinding = GetBuffer(binding.Binding);
					dsa.VertexArrayVertexBuffer(ID, binding.Binding, ((GLBuffer)bufbinding.Buffer).ID, (nint)bufbinding.Range.Offset, (int)binding.Stride);
					switch(binding.InputRate) {
						case VertexInputRate.PerInstance:
							dsa.VertexArrayBindingDivisor(ID, binding.Binding, 1);
							break;
						case VertexInputRate.PerVertex:
						default:
							break;
					}
				}
				if (createInfo.IndexBuffer != null) {
					var ibo = createInfo.IndexBuffer.Value;
					IndexType = GLEnums.Convert(ibo.Type);
					IndexOffset = (nint)ibo.Binding.Range.Offset;
					dsa.VertexArrayElementBuffer(ID, ((GLBuffer)ibo.Binding.Buffer).ID);
				}
			} else {
				// Else fall back to bound vertex arrays
				ID = gl33.GenVertexArrays();
				Graphics.State.BindVertexArray(this);
				// Use vertex array binding if possible
				if (vab != null) {
					foreach (VertexAttrib attrib in Format.Attributes) {
						gl33.EnableVertexAttribArray(attrib.Location);
						vab.VertexAttribBinding(attrib.Location, attrib.Binding);
						var format = GLEnums.StdToGLFormat(attrib.Format);
						if (format == null) throw new ArgumentException($"Cannot convert format {attrib.Format}");
						vab.VertexAttribFormat(attrib.Location, format.Count, format.Type, attrib.Format.IsNumberFormatNormalized, attrib.Offset);
					}
					foreach (VertexBinding binding in Format.Bindings) {
						BufferBinding bufbinding = GetBuffer(binding.Binding);
						vab.BindVertexBuffer(binding.Binding, ((GLBuffer)bufbinding.Buffer).ID, (nint)bufbinding.Range.Offset, (int)binding.Stride);
						switch (binding.InputRate) {
							case VertexInputRate.PerInstance:
								vab.VertexBindingDivisor(binding.Binding, 1);
								break;
							case VertexInputRate.PerVertex:
							default:
								break;
						}
					}
					if (createInfo.IndexBuffer != null) {
						var ibo = createInfo.IndexBuffer.Value;
						IndexType = GLEnums.Convert(ibo.Type);
						IndexOffset = (nint)ibo.Binding.Range.Offset;
						var id = ((GLBuffer)ibo.Binding.Buffer).ID;
						gl33.BindBuffer(GLBufferTarget.ElementArray, id);
						Graphics.State.SetBoundBuffer(GLBufferTarget.ElementArray, id);
					}
				} else {
					// Else fall back to old school GL_ARRAY_BUFFER + glVertexAttribPointer specification
					foreach(var binding in Format.Bindings) {
						BufferBinding bufbinding = GetBuffer(binding.Binding);
						Graphics.State.BindBuffer(GLBufferTarget.Array, ((GLBuffer)bufbinding.Buffer).ID);
						foreach (var attrib in Format.Attributes) {
							if (attrib.Binding == binding.Binding) {
								var format = GLEnums.StdToGLFormat(attrib.Format);
								if (format == null) throw new ArgumentException($"Cannot convert format {attrib.Format}");
								gl33.VertexAttribPointer(attrib.Location, format.Count, format.Type, attrib.Format.IsNumberFormatNormalized, (int)binding.Stride, (nint)(bufbinding.Range.Offset + attrib.Offset));
								switch(binding.InputRate) {
									case VertexInputRate.PerInstance:
										gl33.VertexAttribDivisor(attrib.Location, 1);
										break;
									case VertexInputRate.PerVertex:
									default:
										break;
								}
							}
						}
					}
				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			GL.GL33!.DeleteVertexArrays(ID);
			Graphics.State.InvalidateVertexArrayID(ID);
		}

	}
}
