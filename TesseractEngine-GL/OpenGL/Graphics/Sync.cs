using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {
	
	public class GLSync : ISync {

		public GLGraphics Graphics { get; }

		public nuint SyncID { get; private set; }

		public bool IsFence { get; }


		public SyncGranularity Granularity => throw new NotImplementedException();

		public SyncDirection Direction => throw new NotImplementedException();

		public SyncFeatures Features => throw new NotImplementedException();

		public GLSync(GLGraphics graphics, SyncCreateInfo createInfo) {
			Graphics = graphics;

		}

		public void GenerateFence() {
			var sync = Graphics.GL.ARBSync!;
			if (SyncID != 0) sync.DeleteSync(SyncID);
			SyncID = sync.FenceSync(GLSyncCondition.GPUCommandsComplete);
		}

		public bool HostWait(ulong timeout) {
			if (SyncID != 0) {
				GLWaitResult result = Graphics.GL.ARBSync!.ClientWaitSync(SyncID, GLSyncFlags.FlushCommands, timeout);
				switch(result) {
					case GLWaitResult.AlreadySignaled:
					case GLWaitResult.ConditionSatisfied:
						return false;
					case GLWaitResult.TimeoutExpired:
						return true;
					default:
						throw new GLException($"Error waiting for fence: {Graphics.GL.GL11.GetError()}");
				}
			} else return false;
		}

		public void HostSet() => throw new InvalidOperationException("HostSet operation not supported for OpenGL fence objects");

		public void HostReset() {
			if (SyncID != 0) {
				Graphics.GL.ARBSync!.DeleteSync(SyncID);
				SyncID = 0;
			}
		}

		public bool HostPoll() {
			if (SyncID == 0) return false;
			uint status = (uint)Graphics.GL.ARBSync!.GetSync(SyncID, GLGetSync.SyncStatus);
			switch(status) {
				case Native.GLEnums.GL_SIGNALED:
					return true;
				case Native.GLEnums.GL_UNSIGNALED:
					return false;
				default:
					throw new GLException($"Bad return value from GetSync: {status}");
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			var sync = Graphics.GL.ARBSync;
			if (SyncID != 0) sync!.DeleteSync(SyncID);
		}

	}
}
