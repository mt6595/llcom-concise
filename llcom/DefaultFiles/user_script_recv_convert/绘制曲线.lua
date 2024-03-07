--The Graph tool supports 10 sets of graphics, numbered 0-9

local data = uartData:split("\r\n")

for i=1,#data do
    local temp = tonumber(data[i])
    if temp then
        apiAddPoint(temp,0)
    end
end

--return raw data
return uartData

