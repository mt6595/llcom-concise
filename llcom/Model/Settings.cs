using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llcom.Model
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    class Settings
    {
        public event EventHandler MainWindowTop;
        private string _dataToSend = "uart data";
        private int _baudRate = 115200;
        private bool _autoReconnect = true;
        private bool _autoSaveLog = true;
        private int _showHexFormat = 0;
        private bool _hexSend = false;
        private bool _subpackageShow = true;
        private bool _showSend = true;
        private int _parity = 0;
        private int _timeout = 20;
        private int _dataBits = 8;
        private int _stopBit = 1;
        private string _sendScript = "default";
        private string _recvScript = "default";
        private string _runScript = "example";
        private bool _topmost = false;
        public List<List<ToSendData>> quickSendList = new List<List<ToSendData>>();
        private int _quickSendSelect = -1;
        private bool _autoUpdate = true;
        private uint _maxLength = 10240;
        private string _language = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
        private int _encoding = 65001;
        private bool _terminal = true;
        private bool _extraEnter = false;

        //窗口大小与位置
        private double _windowTop = 0;
        public double windowTop { get { return _windowTop; } set { _windowTop = value; SaveConfig(); } }
        private double _windowLeft = 0;
        public double windowLeft { get { return _windowLeft; } set { _windowLeft = value; SaveConfig(); } }
        private double _windowWidth = 0;
        public double windowWidth { get { return _windowWidth; } set { _windowWidth = value; SaveConfig(); } }
        private double _windowHeight = 0;
        public double windowHeight { get { return _windowHeight; } set { _windowHeight = value; SaveConfig(); } }

        public int SentCount { get; set; } = 0;
        public int ReceivedCount { get; set; } = 0;

        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            File.WriteAllText(Tools.Global.ProfilePath+"settings.json", JsonConvert.SerializeObject(this));
        }

        /// <summary>
        /// 串口接收每包最大长度
        /// </summary>
        public uint maxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                _maxLength = value;
                SaveConfig();
            }
        }

        /// <summary>
        /// 当前选中的快捷发送列表数据
        /// </summary>
        public List<ToSendData> quickSend
        {
            get
            {
                if (_quickSendSelect < 0 || _quickSendSelect > 10)
                    return new List<ToSendData>();
                if (quickSendList.Count <= 10)
                {
                    for (var i = 0; i < 10; i++)
                        quickSendList.Add(new List<ToSendData>());
                }
                return quickSendList[_quickSendSelect];
            }
            set
            {
                if (_quickSendSelect < 0 || _quickSendSelect > 10)
                    return;
                if (quickSendList.Count <= 10)
                {
                    for (var i = 0; i < 10; i++)
                        quickSendList.Add(new List<ToSendData>());
                }
                quickSendList[_quickSendSelect] = value;
                SaveConfig();
            }
        }

        /// <summary>
        /// 当前选中的快速发送列表编号
        /// </summary>
        public int quickSendSelect
        {
            get
            {
                return _quickSendSelect;
            }
            set
            {
                _quickSendSelect = value;
                SaveConfig();
            }
        }

        /// <summary>
        /// 是否开启自动升级
        /// </summary>
        public bool autoUpdate
        {
            get
            {
                return _autoUpdate;
            }
            set
            {
                _autoUpdate = value;
                SaveConfig();
            }
        }

        public string dataToSend
        {
            get
            {
                return _dataToSend;
            }
            set
            {
                _dataToSend = value;
                SaveConfig();
            }
        }
        public int baudRate
        {
            get
            {
                return _baudRate;
            }
            set
            {
                try
                {
                    Tools.Global.uart.serial.BaudRate = value;
                    _baudRate = value;
                    SaveConfig();
                }
                catch(Exception e)
                {
                    Tools.MessageBox.Show(e.Message);
                }
            }
        }

        public bool autoReconnect
        {
            get
            {
                return _autoReconnect;
            }
            set
            {
                _autoReconnect = value;
                SaveConfig();
            }
        }

        public bool autoSaveLog
        {
            get
            {
                return _autoSaveLog;
            }
            set
            {
                _autoSaveLog = value;
                SaveConfig();
            }
        }

        /// <summary>
        /// 串口数据显示格式
        /// 0 都显示
        /// 1 只显示字符串
        /// 2 只显示Hex
        /// </summary>
        public int showHexFormat
        {
            get
            {
                return _showHexFormat;
            }
            set
            {
                _showHexFormat = value;
                SaveConfig();
            }
        }

        /// <summary>
        /// 主数据发送框是否发hex
        /// </summary>
        public bool hexSend
        {
            get
            {
                return _hexSend;
            }
            set
            {
                _hexSend = value;
                SaveConfig();
            }
        }

        /// <summary>
        /// 是否需要时间戳分包显示
        /// </summary>
        public bool subpackageShow
        {
            get
            {
                return _subpackageShow;
            }
            set
            {
                _subpackageShow = value;
                SaveConfig();
            }
        }

        public bool showSend
        {
            get
            {
                return _showSend;
            }
            set
            {
                _showSend = value;
                SaveConfig();
            }
        }

        public int parity
        {
            get
            {
                return _parity;
            }
            set
            {
                try
                {
                    _parity = value;
                    Tools.Global.uart.serial.Parity = (Parity)value;
                    SaveConfig();
                }
                catch (Exception e)
                {
                    Tools.MessageBox.Show(e.Message);
                }
            }
        }

        public int timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
                SaveConfig();
            }
        }

        public int dataBits
        {
            get
            {
                return _dataBits;
            }
            set
            {
                try
                {
                    _dataBits = value;
                    Tools.Global.uart.serial.DataBits = value;
                    SaveConfig();
                }
                catch (Exception e)
                {
                    Tools.MessageBox.Show(e.Message);
                }
            }
        }

        public int stopBit
        {
            get
            {
                return _stopBit;
            }
            set
            {
                try
                {
                    _stopBit = value;
                    Tools.Global.uart.serial.StopBits = (StopBits)value;
                    SaveConfig();
                }
                catch (Exception e)
                {
                    Tools.MessageBox.Show(e.Message);
                }
            }
        }

        public string sendScript
        {
            get
            {
                return _sendScript;
            }
            set
            {
                _sendScript = value;
                SaveConfig();
            }
        }

        public string recvScript
        {
            get
            {
                return _recvScript;
            }
            set
            {
                _recvScript = value;
                SaveConfig();
            }
        }

        public string runScript
        {
            get
            {
                return _runScript;
            }
            set
            {
                _runScript = value;
                SaveConfig();
            }
        }

        public bool topmost
        {
            get
            {
                return _topmost;
            }
            set
            {
                _topmost = value;
                try
                {
                    MainWindowTop(value, EventArgs.Empty);
                }
                catch { }
                SaveConfig();
            }
        }

        public string language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
                Tools.Global.LoadLanguageFile(value);
                SaveConfig();
            }
        }

        public int encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                try
                {
                    Encoding.GetEncoding(value);
                    _encoding = value;
                    SaveConfig();
                }
                catch { }//获取出错说明编码不对
            }
        }

        public bool extraEnter
        {
            get
            {
                return _extraEnter;
            }
            set
            {
                _extraEnter = value;
                SaveConfig();
            }
        }

        public bool DisableLog { get; set; } = false;

        private string _quickListName0 = "Quick Group 0";
        public string quickListName0 { get { return _quickListName0; } set { _quickListName0 = value; SaveConfig(); } }

        private string _quickListName1 = "Quick Group 1";
        public string quickListName1 { get { return _quickListName1; } set { _quickListName1 = value; SaveConfig(); } }

        private string _quickListName2 = "Quick Group 2";
        public string quickListName2 { get { return _quickListName2; } set { _quickListName2 = value; SaveConfig(); } }

        private string _quickListName3 = "Quick Group 3";
        public string quickListName3 { get { return _quickListName3; } set { _quickListName3 = value; SaveConfig(); } }

        private string _quickListName4 = "Quick Group 4";
        public string quickListName4 { get { return _quickListName4; } set { _quickListName4 = value; SaveConfig(); } }

        private string _quickListName5 = "Quick Group 5";
        public string quickListName5 { get { return _quickListName5; } set { _quickListName5 = value; SaveConfig(); } }

        private string _quickListName6 = "Quick Group 6";
        public string quickListName6 { get { return _quickListName6; } set { _quickListName6 = value; SaveConfig(); } }

        private string _quickListName7 = "Quick Group 7";
        public string quickListName7 { get { return _quickListName7; } set { _quickListName7 = value; SaveConfig(); } }

        private string _quickListName8 = "Quick Group 8";
        public string quickListName8 { get { return _quickListName8; } set { _quickListName8 = value; SaveConfig(); } }

        private string _quickListName9 = "Quick Group 9";
        public string quickListName9 { get { return _quickListName9; } set { _quickListName9 = value; SaveConfig(); } }

        public string GetQuickListNameNow()
        {
            return _quickSendSelect switch
            {
                0 => quickListName0,
                1 => quickListName1,
                2 => quickListName2,
                3 => quickListName3,
                4 => quickListName4,
                5 => quickListName5,
                6 => quickListName6,
                7 => quickListName7,
                8 => quickListName8,
                9 => quickListName9,
                _ => "??",
            };
        }
        public void SetQuickListNameNow(string name)
        {
            switch (_quickSendSelect)
            {
                case 0:
                    quickListName0 = name;
                    break;
                case 1:
                    quickListName1 = name;
                    break;
                case 2:
                    quickListName2 = name;
                    break;
                case 3:
                    quickListName3 = name;
                    break;
                case 4:
                    quickListName4 = name;
                    break;
                case 5:
                    quickListName5 = name;
                    break;
                case 6:
                    quickListName6 = name;
                    break;
                case 7:
                    quickListName7 = name;
                    break;
                case 8:
                    quickListName8 = name;
                    break;
                case 9:
                    quickListName9 = name;
                    break;
                default:
                    break;
            }
        }

        private int _maxPackShow = 1024*2;
        /// <summary>
        /// log显示时，一包最大显示长度
        /// </summary>
        public int MaxPackShow { get { return _maxPackShow; } set { _maxPackShow = value; SaveConfig(); } }

        private int _maxPacksAutoClear = 200;
        /// <summary>
        /// log显示时，达到多少包自动清空日志区域
        /// </summary>
        public int MaxPacksAutoClear { get { return _maxPacksAutoClear; } set { _maxPacksAutoClear = value; SaveConfig(); } }

        private bool _lagAutoClear = true;
        /// <summary>
        /// log输出卡顿时，自动清空数据
        /// </summary>
        public bool LagAutoClear { get { return _lagAutoClear; } set { _lagAutoClear = value; SaveConfig(); } }
    }
}
