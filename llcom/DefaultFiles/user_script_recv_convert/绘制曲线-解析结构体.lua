--The Graph tool supports 10 sets of graphics, numbered 0-9

local data = uartData:split(string.char(0x0A))
--Data Framework:[uint8_t data][int32_t data][float data][0x0A]

for i=1,#data do
    if #data[i] == (1+4+4) then
        --Split by structure, reference https://cloudwu.github.io/lua53doc/manual.html#6.4.2
        local u8,i32,f32 = string.unpack("<Blf",data[i])
        apiAddPoint(u8,0)
        apiAddPoint(i32,1)
        apiAddPoint(f32,2)
    end
end

--return raw data
return uartData

