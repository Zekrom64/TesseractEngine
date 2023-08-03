namespace Tesseract.Core.Utilities.Data {
	public enum DataType : byte {
		None = StructuredData.TypeNone,

		Boolean = StructuredData.TypeBoolean,

		Byte = StructuredData.TypeByte,
		Int = StructuredData.TypeInt,
		Long = StructuredData.TypeLong,
		Float = StructuredData.TypeFloat,
		Double = StructuredData.TypeDouble,

		String = StructuredData.TypeString,
		List = StructuredData.TypeList,
		Object = StructuredData.TypeObject,
		ObjectStreaming = StructuredData.TypeObjectStreaming
	}

}
