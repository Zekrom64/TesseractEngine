using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {
	
	public class GLSync : ISync, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public nuint SyncID { get; private set; }

		public bool IsFence { get; }


		public SyncGranularity Granularity => throw new NotImplementedException();

		public SyncDirection Direction => throw new NotImplementedException();

		public SyncFeatures Features => throw new NotImplementedException();

		public GLSync(GLGraphics graphics, SyncCreateInfo createInfo) {
			Graphics = graphics;

		}

		public void GenerateFence() {
			var gl33 = GL.GL33!;
			if (SyncID != 0) gl33.DeleteSync(SyncID);
			SyncID = gl33.FenceSync(GLSyncCondition.GPUCommandsComplete);
		}

		public bool HostWait(ulong timeout) {
			if (SyncID != 0) {
				GLWaitResult result = GL.GL33!.ClientWaitSync(SyncID, GLSyncFlags.FlushCommands, timeout);
				return result switch {
					GLWaitResult.AlreadySignaled or GLWaitResult.ConditionSatisfied => false,
					GLWaitResult.TimeoutExpired => true,
					_ => throw new GLException($"Error waiting for fence: {GL.GL11.GetError()}"),
				};
			} else return false;
		}

		public void HostSet() => throw new InvalidOperationException("HostSet operation not supported for OpenGL fence objects");

		public void HostReset() {
			if (SyncID != 0) {
				GL.ARBSync!.DeleteSync(SyncID);
				SyncID = 0;
			}
		}

		public bool HostPoll() {
			if (SyncID == 0) return false;
			uint status = (uint)GL.ARBSync!.GetSync(SyncID, GLGetSync.SyncStatus);
			return status switch {
				Native.GLEnums.GL_SIGNALED => true,
				Native.GLEnums.GL_UNSIGNALED => false,
				_ => throw new GLException($"Bad return value from GetSync: {status}"),
			};
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			var sync = GL.ARBSync;
			if (SyncID != 0) sync!.DeleteSync(SyncID);
		}

	}
}
