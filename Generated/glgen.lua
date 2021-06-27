require "io"

inf = io.open("glew.h", "r")
outf = io.open("glenums.out.txt", "w")

local emitted = false
while true do
	local line = inf:read()
	if not line then break end
	if #line > 0 then
		if line:sub(1,#"#define GL_") == "#define GL_" then
			line = line:sub((#"#define ")+1)
			local pos = line:find(' ')
			local label = line:sub(1,pos-1)
			if label:upper() == label then
				local value = line:sub(pos+1)
				outf:write("public const GLenum " .. label .. " = " .. value .. ";\n")
				emitted = true
			end
		else
			if emitted then
				outf:write("\n")
				emitted = false
			end
		end
	end
end

inf:close()
outf:close()