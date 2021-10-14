using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {

	public class AVDictionary : IDisposable {

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Variable is modified indirectly by pointer in the libav API")]
		private IntPtr dictionary;
		[NativeType("AVDictionary*")]
		public IntPtr Dictionary => dictionary;

		public AVDictionary([NativeType("AVDictionary*")] IntPtr pDictionary) {
			dictionary = pDictionary;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (dictionary != IntPtr.Zero) {
				unsafe {
					fixed(IntPtr* pDictionary = &dictionary) {
						LibAVUtil.Functions.av_dict_free((IntPtr)pDictionary);
					}
				}
			}
		}

		~AVDictionary() {
			Dispose();
		}

		public IConstPointer<AVDictionaryEntry> Get(string key, IConstPointer<AVDictionaryEntry> prev = null, AVDictionaryFlags flags = 0) {
			IntPtr pEntry = LibAVUtil.Functions.av_dict_get(dictionary, key, prev != null ? prev.Ptr : IntPtr.Zero, flags);
			return pEntry != IntPtr.Zero ? new ManagedPointer<AVDictionaryEntry>(pEntry) : null;
		}

		public AVError Set(string key, string value, AVDictionaryFlags flags = 0) {
			flags &= ~(AVDictionaryFlags.DontStrdupKey | AVDictionaryFlags.DontStrdupVal); // Don't allow shennanigans with marshaled string memory
			return LibAVUtil.Functions.av_dict_set(dictionary, key, value, flags);
		}

		public string this[string key] {
			get => Get(key).Value.Value;
			set {
				AVError err = Set(key, value);
				if (err != AVError.None) throw new AVException("Failed to set dictionary entry", err);
			}
		}

		public AVError Parse(string str, string keyValSep, string pairsSep, AVDictionaryFlags flags = 0) {
			flags &= ~(AVDictionaryFlags.DontStrdupKey | AVDictionaryFlags.DontStrdupVal); // Don't allow shennanigans with marshaled string memory
			unsafe {
				fixed(IntPtr* pDictionary = &dictionary) {
					return LibAVUtil.Functions.av_dict_parse_string((IntPtr)pDictionary, str, keyValSep, pairsSep, flags);
				}
			}
		}

		public int Count => LibAVUtil.Functions.av_dict_count(dictionary);

	}

}
