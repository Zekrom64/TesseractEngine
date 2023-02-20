add_library (VulkanMemoryAllocator SHARED "main.cpp"  "vk_mem_alloc.h")

target_include_directories(VulkanMemoryAllocator PUBLIC $ENV{VULKAN_SDK}/Include)

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET VulkanMemoryAllocator PROPERTY CXX_STANDARD 20)
endif()

