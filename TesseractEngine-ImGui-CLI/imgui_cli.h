#pragma once

#include <cstdlib>
#include <vector>

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Text;
using namespace System::Runtime::InteropServices;

class StringParam {
private:
	union {
		char m_buffer[128 - sizeof(size_t)];
		char* m_pointer;
	};
	size_t m_length;
public:
	inline StringParam() {
		m_length = SIZE_MAX;
		m_pointer = nullptr;
	}

	StringParam(String^ mstr, bool escapeFmt = false) {
		if (mstr) { // If non-null allocate string
			// If ImGui treats this as a 'format' string and it contains a percentage, escape the percentage to prevent crashing
			if (escapeFmt && mstr->Contains('%')) mstr = mstr->Replace("%", "%%");
			auto utf8 = Encoding::UTF8;
			m_length = utf8->GetByteCount(mstr);
			if (is_allocd()) { // If requires allocation create via marshal
				m_pointer = (char*)(void*)Marshal::StringToCoTaskMemUTF8(mstr);
			} else { // Else encode in the internal buffer
				utf8->GetBytes(mstr, Span<unsigned char>((void*)m_buffer, sizeof(m_buffer)));
				m_buffer[m_length] = 0;
			}
		} else { // Else if null set internal fields accordingly
			m_length = SIZE_MAX;
			m_pointer = nullptr;
		}
	}

	inline ~StringParam() {
		if (is_allocd() && m_length != SIZE_MAX) Marshal::FreeCoTaskMem((IntPtr)m_pointer);
	}

	StringParam(const StringParam&) = delete;
	StringParam& operator=(const StringParam&) = delete;

	StringParam& operator=(String^ mstr) {
		// Destroy and reconstruct for assignment
		this->~StringParam();
		new (this) StringParam(mstr);
		return *this;
	}

	inline size_t length() const {
		if (m_length == SIZE_MAX) return 0;
		else return m_length;
	}

	inline bool is_allocd() const {
		return m_length > sizeof(m_buffer) - 1;
	}

	inline const char* c_str() const {
		if (is_allocd()) return m_pointer;
		else return m_buffer;
	}

	inline const char* begin() const {
		return c_str();
	}

	inline const char* end() const {
		return c_str() + length();
	}

	int to_index(const char* ptr) const {
		if (ptr < begin() || ptr >= end()) return -1;
		return Encoding::UTF8->GetCharCount((unsigned char*)begin(), (int)(ptr - begin()));
	}
};

class StringArrayParam {
private:
	static constexpr size_t N_STACKALLOC_ELEMS = 32;
	static constexpr size_t N_STACKALLOC_CHARS = 1024;

	char m_charbuf[N_STACKALLOC_CHARS];
	size_t m_charcount;

	const char* m_strbuf[N_STACKALLOC_ELEMS];
	size_t m_count;
	
	std::vector<const char*> m_strvec;


	void add(System::String^ str) {
		auto utf8 = Encoding::UTF8;

		const char* ptr = nullptr;
		if (str) { // If non null allocate string
			size_t size = (size_t)utf8->GetByteCount(str) + 1;
			if (size > (sizeof(m_charbuf) - m_charcount)) { // If larger than the internal buffer, allocate via marshal
				ptr = (const char*)(void*)Marshal::StringToCoTaskMemUTF8(str);
			} else { // Else encode in internal buffer
				ptr = m_charbuf + m_charcount;
				m_charcount += size;
				utf8->GetBytes(str, Span<unsigned char>((void*)ptr, (int)size));
			}
		}

		if (m_count < N_STACKALLOC_ELEMS) { // If possible put in internal string array
			m_strbuf[m_count++] = ptr;
		} else { // Else put in vector
			// If adding past the internal array size, copy to the vector first
			if (m_count == N_STACKALLOC_ELEMS) {
				m_strvec.reserve(N_STACKALLOC_ELEMS + 1);
				for (size_t i = 0; i < N_STACKALLOC_ELEMS; i++) m_strvec.push_back(m_strbuf[i]);
			}
			m_strvec.push_back(ptr);
			m_count++;
		}
	}

	void release(const char* ptr) {
		// Free if pointer is outside the buffer and non-null
		if (ptr && (ptr < m_charbuf || ptr >= (m_charbuf + sizeof(m_charbuf))))
			Marshal::FreeCoTaskMem((IntPtr)(void*)ptr);
	}

public:
	inline StringArrayParam(System::Collections::Generic::IEnumerable<System::String^>^ enumerable) {
		memset(m_charbuf, 0, sizeof(m_charbuf));
		m_charcount = 0;
		memset(m_strbuf, 0, sizeof(m_strbuf));
		m_count = 0;

		auto e = enumerable->GetEnumerator();
		while (e->MoveNext()) add(e->Current);
	}

	~StringArrayParam() {
		// Release string pointers
		const char* const* pData = data();
		for (size_t i = 0; i < m_count; i++) release(*pData++);
	}

	StringArrayParam(const StringArrayParam&) = delete;
	StringArrayParam& operator=(const StringArrayParam&) = delete;

	inline size_t length() const {
		return m_count;
	}

	inline const char* const* data() const {
		if (m_count > N_STACKALLOC_ELEMS) return m_strvec.data();
		else return m_strbuf;
	}
};

ref class StringHolder {
private:
	System::String^ m_mstr = nullptr;
	System::IntPtr m_strptr = System::IntPtr::Zero;
	int m_strlen = 0;

	void release() {
		m_mstr = nullptr;
		if (m_strptr != System::IntPtr::Zero) {
			Marshal::FreeCoTaskMem(m_strptr);
			m_strptr = System::IntPtr::Zero;
		}
		m_strlen = 0;
	}

internal:
	const char* c_str() {
		return (const char*)(void*)m_strptr;
	}

	const char* begin() {
		return c_str();
	}

	const char* end() {
		return c_str() + m_strlen;
	}

	void Set(const char* str) {
		if (str == (const char*)(void*)m_strptr) return;
		Set(Tesseract::Core::Native::MemoryUtil::GetUTF8((System::IntPtr)(void*)str, -1, true));
	}

public:
	StringHolder() {}

	StringHolder(System::String^ mstr) {
		Set(mstr);
	}

	~StringHolder() {
		release();
	}

	void Set(System::String^ mstr) {
		if (mstr == m_mstr) return;
		release();
		if (mstr) {
			m_mstr = mstr;
			m_strptr = Marshal::StringToCoTaskMemUTF8(mstr);
			m_strlen = Encoding::UTF8->GetByteCount(mstr);
		}
	}

	System::String^ Get() {
		return m_mstr;
	}

};

/*
generic<typename T>
ref class ImVectorCLI : Tesseract::ImGui::IImVector<T> {
internal:

public:
	// Inherited via IEnumerable
	virtual System::Collections::IEnumerator^ GetEnumeratorBase() = System::Collections::IEnumerable::GetEnumerator {
		throw gcnew System::NotImplementedException();
		// // O: insert return statement here
	}

	// Inherited via ICollection
	virtual System::Collections::Generic::IEnumerator<T>^ GetEnumerator()
	{
		throw gcnew System::NotImplementedException();
		// // O: insert return statement here
	}

	// Inherited via IList
	virtual property int Count;
	virtual property bool IsReadOnly;
	virtual void Add(T item)
	{
		throw gcnew System::NotImplementedException();
	}
	virtual void Clear()
	{
		throw gcnew System::NotImplementedException();
	}
	virtual bool Contains(T item)
	{
		return false;
	}
	virtual void CopyTo(array<T, 1>^ array, int arrayIndex)
	{
		throw gcnew System::NotImplementedException();
	}
	virtual bool Remove(T item)
	{
		return false;
	}

	// Inherited via IImVector
	virtual int IndexOf(T item)
	{
		return 0;
	}
	virtual void Insert(int index, T item)
	{
		throw gcnew System::NotImplementedException();
	}
	virtual void RemoveAt(int index)
	{
		throw gcnew System::NotImplementedException();
	}
	virtual System::Span<T> AsSpan()
	{
		return System::Span<T>();
	}
	virtual void Resize(int newSize)
	{
		throw gcnew System::NotImplementedException();
	}

	virtual property T default[int] {
		virtual T get(int index) {

		}
		virtual void set(int index, T value) {

		}
	}
};
*/
