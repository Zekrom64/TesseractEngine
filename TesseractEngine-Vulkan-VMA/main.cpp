// Empty file just to instantiate the functions in the header
// Requires some defines before including the header to make VMA cooperate

// This C++ file will include the function definitions
#define VMA_IMPLEMENTATION
// Enable the use of dynamic function loading instead of linking against a DLL
#define VMA_DYNAMIC_VULKAN_FUNCTIONS 1
#define VMA_STATIC_VULKAN_FUNCTIONS 0
#ifdef _WIN32
#define VMA_CALL_PRE __declspec(dllexport)
#else
#define VMA_CALL_PRE __attribute__((visibility("default")))
#endif
#define VMA_CALL_POST __cdecl

#include "vk_mem_alloc.h"
