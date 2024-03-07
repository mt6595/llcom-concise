--The Graph tool supports 10 sets of graphics, numbered 0-9

local data = uartData:split("\r\n")
for i=1,#data do
    local temp = data[i]:split(",")
    if #temp == 2 then
        local n1 = tonumber(temp[1])
        local n2 = tonumber(temp[2])
        if n1 and n2 then
            apiAddPoint(n1,0)
            apiAddPoint(n2,1)
        end
    end
end

--return raw data
return uartData

