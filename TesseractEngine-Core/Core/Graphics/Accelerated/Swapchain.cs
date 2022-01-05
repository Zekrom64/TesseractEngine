using System;
using Tesseract.Core.Math;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// Enumeration of types of swapchain images.
	/// </summary>
	public enum SwapchainImageType {
		/// <summary>
		/// The swapchain images are individual textures.
		/// </summary>
		Texture,
		/// <summary>
		/// The swapchain images are prebuilt framebuffers.
		/// </summary>
		Framebuffer
	}

	/// <summary>
	/// Enumeration of swapchain presentation modes. These determine how images/frames are managed
	/// by the swapchain.
	/// </summary>
	public enum SwapchainPresentMode {
		/// <summary>
		/// Images are acquired and presented in a first-in-first-out manner; images are acquired
		/// sequentially and will be presented seqentially, blocking until an image is available or
		/// presentation is ready. This is equivalent to a "V-sync enabled" mode, and screen tearing
		/// will be prevented but if a frame is late for presentation submission will block until
		/// the next refresh period.
		/// </summary>
		FIFO,
		/// <summary>
		/// Images are acquired and presented immediately, regardless of whether presentation has
		/// finished on the previous frame. This puts no limit on how fast frames can be presented
		/// but will likely cause screen tearing. This is equivalent to a "V-sync disabled" mode.
		/// </summary>
		Immediate,
		/// <summary>
		/// Similar to <see cref="FIFO"/>, but late frame submissions will be accepted immediately.
		/// When presentation is on-time (faster than the refresh rate) this will act like
		/// <see cref="FIFO"/>, but when presentation is late (slower than the refresh rate) this will
		/// act like <see cref="Immediate"/>.
		/// </summary>
		RelaxedFIFO,
		/// <summary>
		/// <para>
		/// Similar to <see cref="FIFO"/>, but a special "mailbox" slot is reserved that stores the
		/// next image for presentation. If a new image is ready for presentation before the currently
		/// presented image is released, it will enter the mailbox and release any image already there
		/// for reuse. This allows rendering at higher framerates than a display may be able to present
		/// without causing screen tearing. This is equivalent to "triple-buffering" in other systems.
		/// </para>
		/// <para>
		///	While some implementations may support using this explicitly if available, some such as
		///	OpenGL will only enable this if requested by the driver and will not publicly advertise
		///	support.
		/// </para>
		/// </summary>
		Mailbox
	}

	/// <summary>
	/// Interface for objects that may be swapchain images.
	/// </summary>
	public interface ISwapchainImage { }

	/// <summary>
	/// A swapchain contains a set of images that are used for presentation to a display surface.
	/// Swapchains are created in an implementation-defined manner for different accelerated
	/// graphics backends, but this interface serves as a backend-agnostic way of using them.
	/// </summary>
	public interface ISwapchain : IDisposable {

		/// <summary>
		/// The size of the images in the swapchain.
		/// </summary>
		public Vector2i Size { get; }

		/// <summary>
		/// The pixel format of the images in the swapchain.
		/// </summary>
		public PixelFormat Format { get; }

		/// <summary>
		/// The type of images used by the swapchain. This will not change between rebuilds.
		/// </summary>
		public SwapchainImageType ImageType { get; }

		/// <summary>
		/// <para>
		/// Event fired when the swapchain has been rebuilt. This will be triggered
		/// by the thread that accesses the swapchain, either in <see cref="BeginFrame"/>
		/// or <see cref="EndFrame"/> depending on how the implementation prefers.
		/// </para>
		/// <para>
		/// Swapchain rebuilding can happen for a number of reasons, most commonly
		/// resizing the swapchain's surface, and there is no easy way to predict
		/// this. When the swapchain is rebuilt the textures belonging to the swapchain
		/// may also be rebuilt, and any resources using them should also be rebuilt
		/// at this time. The textures could be entirely new ones, there could be
		/// a different number of them, and the size and format of them could be
		/// different.
		/// </para>
		/// <para>
		/// This event is invoked in such a way that resource rebuilding can happen
		/// logically; the <see cref="Size"/>, <see cref="Format"/>, and
		/// <see cref="Images"/> properties will contain updated values for
		/// the swapchain. This event will only be invoked when it is certain that
		/// the images are not in use by the swapchain (ie. no frames are awaiting
		/// presentation after <see cref="EndFrame"/>.
		/// </para>
		/// </summary>
		public event Action OnRebuild;

		/// <summary>
		/// The list of all the images in the swapchain. All of the images will have a common
		/// <see cref="Format"/> and <see cref="Size"/>. All textures are effectively images,
		/// having type <see cref="TextureType.Texture2D"/> with a single mipmap level and
		/// array layer, and will have usages enabled by the platform-specific backend. All
		/// framebuffers only guarentee a single color attachment corresponding to the swapchain
		/// image (with the respective format and size).
		/// </summary>
		public ISwapchainImage[] Images { get; }

		/// <summary>
		/// Begins using the next available frame from the swapchain, returning the index
		/// of the image in <see cref="ImageTextures"/> that will be displayed. It is
		/// undefined behavior to call this method again before <see cref="EndFrame(ISync, ISync[])"/>
		/// is called.
		/// </summary>
		/// <param name="signal">Semaphore-like sync object to signal when the frame is ready for use</param>
		/// <returns>Index of the image used by the next frame</returns>
		public int BeginFrame(ISync signal);

		/// <summary>
		/// Ends usage of the most recently acquired frame from <see cref="BeginFrame(ISync[])"/>,
		/// presenting the frame to the surface. The corresponding image may still be in use
		/// for some time after this function is called. Any requested image capturing will be done at some point
		/// after this is called.
		/// </summary>
		/// <param name="signalFence">Fence-like sync object to signal when the frame is no longer in use</param>
		/// <param name="wait">Semaphore-like sync objects to wait for before using the frame</param>
		public void EndFrame(ISync signalFence, params ISync[] wait);

	}

}
