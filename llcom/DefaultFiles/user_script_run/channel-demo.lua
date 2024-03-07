--[[
通用消息通道示例代码
该功能拓展了lua脚本的控制范围
可以更加灵活地进行自动化测试
]]

-- uart，对应软件自身的串口功能
apiSetCb("uart",function (data)
    log.info("uart received",data)
end)
local sendResult = apiSend("uart","ok!")
