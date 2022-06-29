using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {

	public class GLBindSetLayout : IBindSetLayout {

		public IReadOnlyList<BindSetLayoutBinding> Bindings => throw new NotImplementedException();

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}

	public class GLBindPool : IBindPool {

		public GLGraphics Graphics { get; }

		public GLBindPool(GLGraphics graphics, BindPoolCreateInfo createInfo) {
			Graphics = graphics;
		}

		public IBindSet AllocSet(BindSetAllocateInfo allocateInfo) => new GLBindSet(this, allocateInfo);

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}

	public class GLBindSet : IBindSet {

		public GLBindSet(GLBindPool pool, BindSetAllocateInfo allocateInfo) {

		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		public void Update(in ReadOnlySpan<BindSetWrite> writes) { }

	}

}
