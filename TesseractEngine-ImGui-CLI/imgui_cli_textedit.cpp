#include "TextEditor.h"
#include "imgui_cli.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;

namespace Tesseract { namespace CLI { namespace ImGui { namespace Addon {

public ref class ImGuiColorTextEdit {
public:
	enum class PaletteIndex {
		Default,
		Keyword,
		Number,
		String,
		CharLiteral,
		Punctuation,
		Preprocessor,
		Identifier,
		KnownIdentifier,
		PreprocIdentifier,
		Comment,
		MultiLineComment,
		Background,
		Cursor,
		Selection,
		ErrorMarker,
		Breakpoint,
		LineNumber,
		CurrentLineFill,
		CurrentLineFillInactive,
		CurrentLineEdge
	};

	enum class SelectionMode {
		Normal,
		Word,
		Line
	};

	value struct Breakpoint {
		int Line;
		bool Enabled;
		String^ Condition;
	};

	value struct Coordinates {
	public:
		int Line;
		int Column;

		Coordinates(int line, int column) : Line(line), Column(column) {}

		static property Coordinates Invalid {
			Coordinates get() {
				return Coordinates(-1, -1);
			}
		}

		bool operator==(Coordinates c) { return Line == c.Line && Column == c.Column; }
		bool operator!=(Coordinates c) { return Line != c.Line && Column != c.Column; }
		bool operator<(Coordinates c) { return Line != c.Line ? (Line < c.Line) : (Column < c.Column); }
		bool operator>(Coordinates c) { return Line != c.Line ? (Line > c.Line) : (Column > c.Column); }
		bool operator<=(Coordinates c) { return Line != c.Line ? (Line < c.Line) : (Column <= c.Column); }
		bool operator>=(Coordinates c) { return Line != c.Line ? (Line > c.Line) : (Column >= c.Column); }

	internal:
		Coordinates(const TextEditor::Coordinates& c) {
			Line = c.mLine;
			Column = c.mColumn;
		}

		TextEditor::Coordinates convert() { return { Line, Column }; }
	};

	value struct Identifier {
	public:
		Coordinates Location;
		String^ Declaration;

	internal:
		Identifier(const TextEditor::Identifier& i) {
			Location = Coordinates(i.mLocation);
			Declaration = gcnew String(i.mDeclaration.c_str());
		}

		TextEditor::Identifier convert() {
			StringParam decl(Declaration);
			return { Location.convert(), std::string(decl.c_str()) };
		}
	};

	delegate bool TokenizeCallback(ReadOnlySpan<Byte> input, [OutAttribute] int% outBegin, [OutAttribute] int% outEnd, [OutAttribute] PaletteIndex% paletteIndex);

	ref class LanguageDefinition {
	private:
		ref class KeywordsImpl : ICollection<String^> {
		private:
			LanguageDefinition^ m_parent;
			TextEditor::Keywords* m_keywords;

			ref class KeywordsImplEnumerator : IEnumerator<String^> {
			private:
				KeywordsImpl^ m_parent;
				TextEditor::Keywords::iterator* m_iterator;
				String^ m_current;

			public:
				KeywordsImplEnumerator(KeywordsImpl^ parent) {
					m_parent = parent;
					m_iterator = new TextEditor::Keywords::iterator();
					*m_iterator = parent->m_keywords->begin();
				}

				~KeywordsImplEnumerator() {
					delete m_iterator;
				}

				// Inherited via IEnumerator
				virtual property System::Object^ Current2 {
					Object^ get() = System::Collections::IEnumerator::Current::get{
						return Current;
					}
				}

				virtual bool MoveNext() {
					if (*m_iterator == m_parent->m_keywords->end()) return false;
					m_current = gcnew String((*m_iterator)->c_str());
					m_iterator->operator++();
					return true;
				}

				virtual void Reset() {
					*m_iterator = m_parent->m_keywords->begin();
				}

				virtual property System::String^ Current {
					String^ get() {
						if (*m_iterator == m_parent->m_keywords->begin()) throw gcnew InvalidOperationException();
						return m_current;
					}
				}
			};

		internal:
			KeywordsImpl(LanguageDefinition^ parent, TextEditor::Keywords& keywords) {
				m_parent = parent;
				m_keywords = &keywords;
			}

		public:
			// Inherited via IEnumerable
			virtual System::Collections::IEnumerator^ GetEnumeratorBase() = System::Collections::ICollection::GetEnumerator {
				return GetEnumerator();
			}

			// Inherited via ICollection
			virtual IEnumerator<System::String^>^ GetEnumerator() {
				return gcnew KeywordsImplEnumerator(this);
			}
			
			virtual property int Count {
				int get() { return (int)m_keywords->size(); }
			}
			
			virtual property bool IsReadOnly {
				bool get() { return false; }
			}
			
			virtual void AddBase(System::String^ item) = ICollection<String^>::Add {
				StringParam pItem(item);
				m_keywords->emplace(pItem.c_str());
			}
			
			virtual void Clear() { m_keywords->clear(); }
			
			virtual bool Contains(System::String^ item) {
				StringParam pItem(item);
				return m_keywords->find(pItem.c_str()) != m_keywords->end();
			}
			
			virtual void CopyTo(array<System::String^, 1>^ array, int arrayIndex) {
				throw gcnew System::NotImplementedException();
			}

			virtual bool Remove(System::String^ item) {
				StringParam pItem(item);
				auto itr = m_keywords->find(pItem.c_str());
				if (itr == m_keywords->end()) return false;
				m_keywords->erase(itr);
				return true;
			}
		};

		ref class IdentifiersImpl : IDictionary<String^, Identifier> {
		private:
			LanguageDefinition^ m_parent;
			TextEditor::Identifiers* m_identifiers;
			Tesseract::ImGui::Utilities::CLI::DictionaryKeyCollection<String^, Identifier>^ m_keys;
			Tesseract::ImGui::Utilities::CLI::DictionaryValueCollection<String^, Identifier>^ m_values;

			ref class IdentifiersImplEnumerator : IEnumerator<KeyValuePair<String^, Identifier>> {
			private:
				IdentifiersImpl^ m_parent;
				TextEditor::Identifiers::iterator* m_iterator;
				KeyValuePair<String^, Identifier> m_current;

			internal:
				IdentifiersImplEnumerator(IdentifiersImpl^ parent) {
					m_parent = parent;
					m_iterator = new TextEditor::Identifiers::iterator();
					*m_iterator = parent->m_identifiers->begin();
				}

			public:
				~IdentifiersImplEnumerator() {
					delete m_iterator;
				}

				// Inherited via IEnumerator
				virtual property System::Object^ Current2 {
					virtual Object^ get() = System::Collections::IEnumerator::Current::get{
						return Current;
					}
				}

				virtual bool MoveNext() {
					if (*m_iterator == m_parent->m_identifiers->end()) return false;
					auto& current = *m_iterator;
					m_current = KeyValuePair<String^, Identifier>(gcnew String(current->first.c_str()), Identifier(current->second));
					m_iterator->operator++();
					return true;
				}

				virtual void Reset() {
					*m_iterator = m_parent->m_identifiers->begin();
				}

				virtual property KeyValuePair<String^, Identifier>  Current {
					KeyValuePair<String^, Identifier> get() {
						if (*m_iterator == m_parent->m_identifiers->begin()) throw gcnew InvalidOperationException();
						return m_current;
					}
				}

			};

		internal:
			IdentifiersImpl(LanguageDefinition^ parent, TextEditor::Identifiers& identifiers) {
				m_parent = parent;
				m_identifiers = &identifiers;
				m_keys = gcnew Tesseract::ImGui::Utilities::CLI::DictionaryKeyCollection<String^, Identifier>(this);
				m_values = gcnew Tesseract::ImGui::Utilities::CLI::DictionaryValueCollection<String^, Identifier>(this);
			}

		public:
			// Inherited via IEnumerable
			virtual System::Collections::IEnumerator^ GetEnumeratorBase() = System::Collections::IEnumerable::GetEnumerator{
				return GetEnumerator();
			}

			// Inherited via ICollection
			virtual IEnumerator<KeyValuePair<String^, Identifier>>^ GetEnumerator() {
				return gcnew IdentifiersImplEnumerator(this);
			}

			// Inherited via IDictionary
			virtual property int  Count { virtual int get() { return (int)m_identifiers->size(); } }
			virtual property bool  IsReadOnly { virtual bool get() { return false; } }

			virtual void Add(KeyValuePair<String^, Identifier> item) {
				StringParam pKey(item.Key);
				m_identifiers->operator[](pKey.c_str()) = item.Value.convert();
			}

			virtual void Clear() { m_identifiers->clear(); }

			virtual bool Contains(KeyValuePair<String^, Identifier> item) {
				StringParam pKey(item.Key);
				auto itr = m_identifiers->find(pKey.c_str());
				if (itr == m_identifiers->end()) return false;
				return itr->second == item.Value.convert();
			}

			virtual void CopyTo(array<KeyValuePair<String^, Identifier>, 1>^ array, int arrayIndex) {
				for (auto itr = m_identifiers->begin(); itr != m_identifiers->end(); itr++, arrayIndex++) {
					array[arrayIndex] = KeyValuePair<String^, Identifier>(gcnew String(itr->first.c_str()), Identifier(itr->second));
				}
			}

			virtual bool Remove(KeyValuePair<System::String^, Identifier> item) {
				StringParam pKey(item.Key);
				auto itr = m_identifiers->find(pKey.c_str());
				if (itr == m_identifiers->end()) return false;
				if (itr->second != item.Value.convert()) return false;
				m_identifiers->erase(itr);
				return true;
			}

			virtual property Identifier default[String^] {
				Identifier get(String^ key) {
					StringParam pKey(key);
					auto itr = m_identifiers->find(pKey.c_str());
					if (itr == m_identifiers->end()) throw gcnew System::Collections::Generic::KeyNotFoundException();
					return Identifier(itr->second);
				}
				void set(String^ key, Identifier id) {
					StringParam pKey(key);
					m_identifiers->operator[](pKey.c_str()) = id.convert();
				}
			}

			virtual property ICollection<String^>^ Keys {
				ICollection<String^>^ get() { return m_keys; }
			}

			virtual property ICollection<Identifier>^ Values {
				ICollection<Identifier>^ get() { return m_values; }
			}

			virtual void Add(System::String^ key, Identifier value) {
				StringParam pKey(key);
				m_identifiers->operator[](pKey.c_str()) = value.convert();
			}

			virtual bool ContainsKey(System::String^ key) {
				StringParam pKey(key);
				return m_identifiers->find(pKey.c_str()) != m_identifiers->end();
			}

			virtual bool TryGetValue(System::String^ key, Identifier% value) {
				StringParam pKey(key);
				auto itr = m_identifiers->find(pKey.c_str());
				if (itr == m_identifiers->end()) return false;
				value = Identifier(itr->second);
				return true;
			}

			virtual bool Remove(String^ key) {
				StringParam pKey(key);
				auto itr = m_identifiers->find(pKey.c_str());
				if (itr == m_identifiers->end()) return false;
				m_identifiers->erase(itr);
				return true;
			}
		};

		ref class TokenRegexStringsImpl : IList<ValueTuple<String^, PaletteIndex>>, IReadOnlyList<ValueTuple<String^, PaletteIndex>> {
		private:
			LanguageDefinition^ m_parent;
			TextEditor::LanguageDefinition::TokenRegexStrings* m_strings;

		public:
			TokenRegexStringsImpl(LanguageDefinition^ parent, TextEditor::LanguageDefinition::TokenRegexStrings& strings) {
				m_parent = parent;
				m_strings = &strings;
			}

			// Inherited via IEnumerable
			virtual System::Collections::IEnumerator^ GetEnumeratorBase() = System::Collections::IEnumerable::GetEnumerator{
				return GetEnumerator();
			}

			// Inherited via ICollection
			virtual IEnumerator<ValueTuple<String^, PaletteIndex>>^ GetEnumerator() {
				return gcnew Tesseract::ImGui::Utilities::CLI::ListEnumerator<ValueTuple<String^, PaletteIndex>>(this);
			}

			// Inherited via IList
			virtual property int  Count { int get() { return (int)m_strings->size(); } }
			virtual property bool  IsReadOnly { bool get() { return false; } }

			virtual void Add(ValueTuple<String^, PaletteIndex> item) {
				StringParam pKey(item.Item1);
				m_strings->push_back({ std::string(pKey.c_str()), (TextEditor::PaletteIndex)item.Item2 });
			}

			virtual void Clear() { m_strings->clear(); }

			virtual bool Contains(ValueTuple<String^, PaletteIndex> item) { return IndexOf(item) >= 0; }
			
			virtual void CopyTo(array<ValueTuple<String^, PaletteIndex>, 1>^ array, int arrayIndex) {
				for (int i = 0; i < m_strings->size(); i++, arrayIndex++) {
					auto& entry = m_strings->operator[](i);
					array[arrayIndex] = ValueTuple<String^, PaletteIndex>(gcnew String(entry.first.c_str()), (PaletteIndex)entry.second);
				}
			}
			
			virtual bool Remove(ValueTuple<String^, PaletteIndex> item) {
				int index = IndexOf(item);
				if (index < 0) return false;
				RemoveAt(index);
				return true;
			}

			virtual property ValueTuple<String^, PaletteIndex> default[int] {
				ValueTuple<String^, PaletteIndex> get(int index) {
					if (index < 0 || index >= m_strings->size()) throw gcnew IndexOutOfRangeException();
					auto& value = m_strings->operator[](index);
					return ValueTuple<String^, PaletteIndex>(gcnew String(value.first.c_str()), (PaletteIndex)value.second);
				}
				void set(int index, ValueTuple<String^, PaletteIndex> value) {
					if (index < 0 || index >= m_strings->size()) throw gcnew IndexOutOfRangeException();
					StringParam pName(value.Item1);
					m_strings->operator[](index) = { std::string(pName.c_str()), (TextEditor::PaletteIndex)value.Item2 };
				}
			}

			virtual int IndexOf(ValueTuple<String^, PaletteIndex> item) {
				StringParam pName(item.Item1);
				std::string name(pName.c_str());
				for (size_t i = 0; i < m_strings->size(); i++) {
					auto& entry = m_strings->operator[](i);
					if (entry.first != name) continue;
					if (entry.second == (TextEditor::PaletteIndex)item.Item2) return (int)i;
				}
				return -1;
			}

			virtual void Insert(int index, ValueTuple<String^, PaletteIndex> item) {
				StringParam pKey(item.Item1);
				m_strings->insert(m_strings->begin() + index, { std::string(pKey.c_str()), (TextEditor::PaletteIndex)item.Item2 });
			}

			virtual void RemoveAt(int index) {
				m_strings->erase(m_strings->begin() + index);
			}
		};

		TextEditor::LanguageDefinition* m_languageDef;
		KeywordsImpl^ m_keywords;
		IdentifiersImpl^ m_identifiers;
		IdentifiersImpl^ m_preprocIdentifiers;
		TokenizeCallback^ m_tokenize;
		delegate bool TokenizeCallbackNative(const char* in_begin, const char* in_end, const char*& out_begin, const char*& out_end, TextEditor::PaletteIndex& paletteIndex);
		TokenizeCallbackNative^ m_tokenizeNative;
		TokenRegexStringsImpl^ m_tokenRegexStrings;

		bool tokenizeTrampoline(ReadOnlySpan<Byte> input, int% outBegin, int% outEnd, PaletteIndex% paletteIndex) {
			pin_ptr<unsigned char> pInput = &MemoryMarshal::GetReference(input);
			const char* pBase = (const char*)pInput;
			const char* pOutBegin = pBase;
			const char* pOutEnd = pBase;
			TextEditor::PaletteIndex outPalette = TextEditor::PaletteIndex::Default;
			bool ret = m_languageDef->mTokenize(pBase, pBase + input.Length, pOutBegin, pOutEnd, outPalette);
			outBegin = (int)(pOutBegin - pBase);
			outEnd = (int)(pOutEnd - pBase);
			paletteIndex = (PaletteIndex)outPalette;
			return ret;
		}


		bool tokenizeForward(const char* in_begin, const char* in_end, const char*& out_begin, const char*& out_end, TextEditor::PaletteIndex& paletteIndex) {
			ReadOnlySpan<Byte> input((void*)in_begin, (int)(in_end - in_begin));
			int outBegin, outEnd;
			PaletteIndex outPaletteIndex;
			bool ret = m_tokenize->Invoke(input, outBegin, outEnd, outPaletteIndex);
			out_begin = in_begin + outBegin;
			out_end = in_begin + outEnd;
			paletteIndex = (TextEditor::PaletteIndex)outPaletteIndex;
			return false;
		}
	public:
		LanguageDefinition() {
			m_languageDef = new TextEditor::LanguageDefinition();

			m_keywords = gcnew KeywordsImpl(this, m_languageDef->mKeywords);
			m_identifiers = gcnew IdentifiersImpl(this, m_languageDef->mIdentifiers);
			m_preprocIdentifiers = gcnew IdentifiersImpl(this, m_languageDef->mPreprocIdentifiers);
			m_tokenize = nullptr;
			m_tokenizeNative = gcnew TokenizeCallbackNative(this, &LanguageDefinition::tokenizeForward);
			m_tokenRegexStrings = gcnew TokenRegexStringsImpl(this, m_languageDef->mTokenRegexStrings);
		}

		~LanguageDefinition() {
			delete m_languageDef;
		}

	internal:
		LanguageDefinition(const TextEditor::LanguageDefinition& def) : LanguageDefinition() {
			*m_languageDef = def;
			m_tokenize = gcnew TokenizeCallback(this, &LanguageDefinition::tokenizeTrampoline);
		}

	public:
		property String^ Name {
			String^ get() { return gcnew String(m_languageDef->mName.c_str()); }
			void set(String^ value) {
				StringParam pValue(value);
				m_languageDef->mName.assign(pValue.c_str());
			}
		}

		property ICollection<String^>^ Keywords {
			ICollection<String^>^ get() {
				return m_keywords;
			}
		}

		property IDictionary<String^, Identifier>^ Identifiers {
			IDictionary<String^, Identifier>^ get() {
				return m_identifiers;
			}
		}

		property IDictionary<String^, Identifier>^ PreprocIdentifiers {
			IDictionary<String^, Identifier>^ get() {
				return m_preprocIdentifiers;
			}
		}

		property String^ CommentStart {
			String^ get() { return gcnew String(m_languageDef->mCommentStart.c_str()); }
			void set(String^ value) {
				StringParam pValue(value);
				m_languageDef->mCommentStart.assign(pValue.c_str());
			}
		}

		property String^ CommentEnd {
			String^ get() { return gcnew String(m_languageDef->mCommentEnd.c_str()); }
			void set(String^ value) {
				StringParam pValue(value);
				m_languageDef->mCommentEnd.assign(pValue.c_str());
			}
		}

		property String^ SingleLineComment {
			String^ get() { return gcnew String(m_languageDef->mSingleLineComment.c_str()); }
			void set(String^ value) {
				StringParam pValue(value);
				m_languageDef->mSingleLineComment.assign(pValue.c_str());
			}
		}

		property unsigned char PreprocChar {
			unsigned char get() { return m_languageDef->mPreprocChar; }
			void set(unsigned char value) { m_languageDef->mPreprocChar = value; }
		}

		property bool AutoIndentation {
			bool get() { return m_languageDef->mAutoIndentation; }
			void set(bool value) { m_languageDef->mAutoIndentation = value; }
		}

		property TokenizeCallback^ Tokenize {
			TokenizeCallback^ get() { return m_tokenize; }
			void set(TokenizeCallback^ value) {
				m_tokenize = value;
				m_languageDef->mTokenize = (TextEditor::LanguageDefinition::TokenizeCallback)(void*)Marshal::GetFunctionPointerForDelegate(m_tokenizeNative);
			}
		}

		property IList<ValueTuple<String^, PaletteIndex>>^ TokenRegexStrings {
			IList<ValueTuple<String^, PaletteIndex>>^ get() {
				return m_tokenRegexStrings;
			}
		}

		property bool CaseSensitive {
			bool get() { return m_languageDef->mCaseSensitive; }
			void set(bool value) { m_languageDef->mCaseSensitive = value; }
		}

		static property LanguageDefinition^ CPlusPlus {
			LanguageDefinition^ get() {
				return gcnew LanguageDefinition(TextEditor::LanguageDefinition::CPlusPlus());
			}
		}
		static property LanguageDefinition^ HLSL {
			LanguageDefinition^ get() {
				return gcnew LanguageDefinition(TextEditor::LanguageDefinition::HLSL());
			}
		}
		static property LanguageDefinition^ GLSL {
			LanguageDefinition^ get() {
				return gcnew LanguageDefinition(TextEditor::LanguageDefinition::GLSL());
			}
		}
		static property LanguageDefinition^ C {
			LanguageDefinition^ get() {
				return gcnew LanguageDefinition(TextEditor::LanguageDefinition::C());
			}
		}
		static property LanguageDefinition^ SQL {
			LanguageDefinition^ get() {
				return gcnew LanguageDefinition(TextEditor::LanguageDefinition::SQL());
			}
		}
		static property LanguageDefinition^ AngelScript {
			LanguageDefinition^ get() {
				return gcnew LanguageDefinition(TextEditor::LanguageDefinition::AngelScript());
			}
		}
		static property LanguageDefinition^ Lua {
			LanguageDefinition^ get() {
				return gcnew LanguageDefinition(TextEditor::LanguageDefinition::Lua());
			}
		}

	internal:
		TextEditor::LanguageDefinition& getLanguageDef() { return *m_languageDef; }
	};

private:
	::TextEditor* m_textEditor;
	LanguageDefinition^ m_languageDef;

public:
	ImGuiColorTextEdit() {
		m_textEditor = new ::TextEditor();
		m_languageDef = nullptr;
	}

	~ImGuiColorTextEdit() {
		delete m_textEditor;
	}

	property LanguageDefinition^ Language {
		LanguageDefinition^ get() {
			return m_languageDef;
		}
		void set(LanguageDefinition^ def) {
			m_languageDef = def;
			m_textEditor->SetLanguageDefinition(m_languageDef->getLanguageDef());
		}
	}

	property Span<UInt32> Palette {
		Span<UInt32> get() {
			auto& palette = m_textEditor->GetPalette();
			return Span<UInt32>((void*)palette.data(), (int)TextEditor::PaletteIndex::Max);
		}
	}

	void SetErrorMarkers(IReadOnlyDictionary<int, String^>^ markers) {
		TextEditor::ErrorMarkers em;
		auto e = markers->GetEnumerator();
		while (e->MoveNext()) {
			StringParam val(e->Current.Value);
			em[e->Current.Key] = std::string(val.c_str());
		}
		m_textEditor->SetErrorMarkers(em);
	}

	void SetBreakpoints(IReadOnlyCollection<int>^ markers) {
		TextEditor::Breakpoints b;
		auto e = markers->GetEnumerator();
		while (e->MoveNext()) b.emplace(e->Current);
		m_textEditor->SetBreakpoints(b);
	}

	void Render(String^ title, Vector2 size, bool border) {
		StringParam pTitle(title);
		m_textEditor->Render(pTitle.c_str(), { size.X, size.Y }, border);
	}

	property String^ Text {
		String^ get() {
			return gcnew String(m_textEditor->GetText().c_str());
		}
		void set(String^ value) {
			StringParam pValue(value);
			m_textEditor->SetText(pValue.c_str());
		}
	}

	property array<String^>^ TextLines {
		array<String^>^ get() {
			auto lines = m_textEditor->GetTextLines();
			array<String^>^ value = gcnew array<String^>((int)lines.size());
			for (int i = 0; i < lines.size(); i++) value[i] = gcnew String(lines[i].c_str());
			return value;
		}
		void set(array<String^>^ value) {
			std::vector<std::string> lines;
			lines.reserve(value->Length);

			m_textEditor->SetTextLines(lines);
		}
	}

	property String^ SelectedText {
		String^ get() { return gcnew String(m_textEditor->GetSelectedText().c_str()); }
	}

	property String^ CurrentLineText {
		String^ get() { return gcnew String(m_textEditor->GetCurrentLineText().c_str()); }
	}

	property int TotalLines {
		int get() { return m_textEditor->GetTotalLines(); }
	}

	property bool IsOverwrite {
		bool get() { return m_textEditor->IsOverwrite(); }
	}

	property bool IsReadOnly {
		bool get() { return m_textEditor->IsReadOnly(); }
		void set(bool value) { m_textEditor->SetReadOnly(value); }
	}

	property bool IsTextChanged {
		bool get() { return m_textEditor->IsTextChanged(); }
	}

	property bool IsCursorPositionChanged {
		bool get() { return m_textEditor->IsCursorPositionChanged(); }
	}

	property bool IsColorizerEnabled {
		bool get() { return m_textEditor->IsColorizerEnabled(); }
		void set(bool value) { m_textEditor->SetColorizerEnable(value); }
	}

	property Coordinates CursorPosition{
		Coordinates get() {
			auto value = m_textEditor->GetCursorPosition();
			return Coordinates(value.mLine, value.mColumn);
		}
		void set(Coordinates value) {
			m_textEditor->SetCursorPosition({ value.Line, value.Column });
		}
	}

	property bool IsHandleMouseInputsEnabled {
		bool get() { return m_textEditor->IsHandleKeyboardInputsEnabled(); }
		void set(bool value) { m_textEditor->SetHandleMouseInputs(value); }
	}

	property bool IsHandleKeyboardInputsEnabled {
		bool get() { return m_textEditor->IsHandleKeyboardInputsEnabled(); }
		void set(bool value) { m_textEditor->SetHandleKeyboardInputs(value); }
	}

	property bool IsImGuiChildIgnored {
		bool get() { return m_textEditor->IsImGuiChildIgnored(); }
		void set(bool value) { m_textEditor->SetImGuiChildIgnored(value); }
	}

	property bool IsShowingWhitespaces {
		bool get() { return m_textEditor->IsShowingWhitespaces(); }
		void set(bool value) { m_textEditor->SetShowWhitespaces(value); }
	}

	property int TabSize {
		int get() { return m_textEditor->GetTabSize(); }
		void set(int value) { m_textEditor->SetTabSize(value); }
	}

	void InsertText(ReadOnlySpan<unsigned char> value) {
		pin_ptr<unsigned char> pValue = &MemoryMarshal::GetReference(value);
		IM_ASSERT(pValue[value.Length + 1] == '\0');
		m_textEditor->InsertText((const char*)pValue);
	}

	void InsertText(String^ value) {
		StringParam pValue(value);
		m_textEditor->InsertText(pValue.c_str());
	}

	void MoveUp(int amount, bool select) { m_textEditor->MoveUp(amount, select); }
	void MoveUp(int amount) { MoveUp(amount, false); }
	void MoveUp() { MoveUp(1); }

	void MoveDown(int amount, bool select) { m_textEditor->MoveDown(amount, select); }
	void MoveDown(int amount) { MoveDown(amount, false); }
	void MoveDown() { MoveDown(1); }

	void MoveLeft(int amount, bool select, bool wordMode) { m_textEditor->MoveLeft(amount, select, wordMode); }
	void MoveLeft(int amount, bool select) { MoveLeft(amount, select, false); }
	void MoveLeft(int amount) { MoveLeft(amount, false); }
	void MoveLeft() { MoveLeft(1); }

	void MoveRight(int amount, bool select, bool wordMode) { m_textEditor->MoveRight(amount, select, wordMode); }
	void MoveRight(int amount, bool select) { MoveRight(amount, select, false); }
	void MoveRight(int amount) { MoveRight(amount, false); }
	void MoveRight() { MoveRight(1); }

	void MoveTop(bool select) { m_textEditor->MoveTop(select); }
	void MoveTop() { MoveTop(false); }

	void MoveBottom(bool select) { m_textEditor->MoveBottom(select); }
	void MoveBottom() { MoveBottom(false); }

	void MoveHome(bool select) { m_textEditor->MoveHome(select); }
	void MoveHome() { MoveHome(false); }

	void MoveEnd(bool select) { m_textEditor->MoveEnd(select); }
	void MoveEnd() { MoveEnd(false); }

	void SetSelectionStart(Coordinates position) { m_textEditor->SetSelectionStart({ position.Line, position.Column }); }
	void SetSelectionEnd(Coordinates position) { m_textEditor->SetSelectionEnd({ position.Line, position.Column }); }
	void SetSelection(Coordinates start, Coordinates end, SelectionMode mode) { m_textEditor->SetSelection({ start.Line, start.Column }, { end.Line, end.Column }, (TextEditor::SelectionMode)mode); }
	void SetSelection(Coordinates start, Coordinates end) { SetSelection(start, end, SelectionMode::Normal); }
	void SelectWordUnderCursor() { m_textEditor->SelectWordUnderCursor(); }
	void SelectAll() { m_textEditor->SelectAll(); }
	
	property bool HasSelection { bool get() { return m_textEditor->HasSelection(); } }

	void Copy() { m_textEditor->Copy(); }
	void Cut() { m_textEditor->Cut(); }
	void Paste() { m_textEditor->Paste(); }
	void Delete() { m_textEditor->Delete(); }

	property bool CanUndo { bool get() { return m_textEditor->CanUndo(); } }
	property bool CanRedo { bool get() { return m_textEditor->CanRedo(); } }
	void Undo(int steps) { m_textEditor->Undo(steps); }
	void Undo() { Undo(1); }
	void Redo(int steps) { m_textEditor->Redo(steps); }
	void Redo() { Redo(1); }

	static property ReadOnlySpan<UInt32> DarkPalette {
		ReadOnlySpan<UInt32> get() {
			return ReadOnlySpan<UInt32>((void*)TextEditor::GetDarkPalette().data(), (int)TextEditor::PaletteIndex::Max);
		}
	}

	static property ReadOnlySpan<UInt32> LightPalette {
		ReadOnlySpan<UInt32> get() {
			return ReadOnlySpan<UInt32>((void*)TextEditor::GetLightPalette().data(), (int)TextEditor::PaletteIndex::Max);
		}
	}

	static property ReadOnlySpan<UInt32> RetroBluePalette {
		ReadOnlySpan<UInt32> get() {
			return ReadOnlySpan<UInt32>((void*)TextEditor::GetRetroBluePalette().data(), (int)TextEditor::PaletteIndex::Max);
		}
	}
};

}}}}
