using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Graphics.Accelerated {
	
	/// <summary>
	/// A texture layout determines how the content of a texture 
	/// </summary>
	public enum TextureLayout {
		Undefined,
		General,
		ColorAttachment,
		DepthStencilAttachment,
		DepthStencilSampled,
		ShaderSampled,
		TransferSrc,
		TransferDst
	}

}
