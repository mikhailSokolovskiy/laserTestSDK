using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
//CN api for working with fiber lasers
namespace MarkAPI
{
#region 通用宏定义
    public enum BslErrCode
    {
        //所有函数都返回一个整型值
        BSL_ERR_FIFTHFUNCCALLFAILED			=-54,		//第5个子函数调用失败
        BSL_ERR_FOURTHFUNCCALLFAILED		=-53,		//第4个子函数调用失败
        BSL_ERR_THIRDFUNCCALLFAILED			=-52,	    //第3个子函数调用失败
        BSL_ERR_SECONDFUNCCALLFAILED		=-51,		//第2个子函数调用失败
        BSL_ERR_FIRSTFUNCCALLFAILED			=-50,		//第1个子函数调用失败
        BSL_ERR_TENTHPARAMINVALID			=-59,		//第10个参数无效
        BSL_ERR_NINTHPARAMINVALID			=-58,		//第9个参数无效
        BSL_ERR_EIGHTHPARAMINVALID			=-57,		//第8个参数无效
        BSL_ERR_SEVENTHPARAMINVALID			=-56,		//第7个参数无效
        BSL_ERR_SIXTHPARAMINVALID			=-55,		//第6个参数无效
        BSL_ERR_FIFTHPARAMINVALID			=-54,		//第5个参数无效
        BSL_ERR_FOURTHPARAMINVALID			=-53,		//第4个参数无效
        BSL_ERR_THIRDPARAMINVALID			=-52,		//第3个参数无效
        BSL_ERR_SECONDPARAMINVALID			=-51,		//第2个参数无效
        BSL_ERR_FIRSTPARAMINVALID			=-50,		//第1个参数无效
        BSL_ERR_LOADDLLFAILED				=-24,		//加载DLL失败
        BSL_ERR_CALPOINTERROR				=-22,		//检定点小于9个或第一个检定点的X实际值小于0.000001
        BSL_ERR_COVERCALDATAERROR			=-21,		//参数bCover值为false且原参数中已存在检定点数据
        BSL_ERR_ABSENTFILEPATH				=-12,		//文件路径不存在
        BSL_ERR_NOTFINDMARKADAPTOR			=-11,		//无法通过设备ID找到有效的标刻适配器
        BSL_ERR_TARGETSHAPETYPEMISMATCH		=-10,		//指定图元的类型与需求不匹配
        BSL_ERR_NOTFINDTARGETFILE			=-9	,	    //未找到指定文档
        BSL_ERR_EMPTYFILEPATH				=-8	,	    //文件路径为空
        BSL_ERR_NOTFINDTARGETPAR			=-7	,	    //未找到指定参数
        BSL_ERR_UNKNOWNERROR				=-6	,	    //未知错误
        BSL_ERR_NOTFINDTARGETSHAPEINFILE	=-5	,	    //没有在文档中找到指定图元
        BSL_ERR_INDEXOVERFLOW				=-4	,	    //下标值溢出
        BSL_ERR_NOTINITIALIZED				=-3	,	    //全局标刻管理器pGblMarkMgr为NULL
        BSL_ERR_NOSHAPEINFILE				=-2	,	    //文档中没有图元
        BSL_ERR_WRONGPARAM					=-1	,	    //传入的参数错误
        BSL_ERR_SUCCESS						=0	,	    //成功
        BSL_ERR_BSLCADRUN					=1	,	    //发现BslCAD在运行
        BSL_ERR_NOFINDCFGFILE				=2	,	    //找不到BslCAD.CFG
        BSL_ERR_FAILEDOPEN					=3	,	    //打开设备失败
        BSL_ERR_NODEVICE					=4	,	    //没有有效的lmc1设备
        BSL_ERR_HARDVER						=5	,	    //设备版本错误
        BSL_ERR_DEVCFG						=6	,	    //找不到设备配置文件
        BSL_ERR_STOPSIGNAL					=7	,	    //报警信号
        BSL_ERR_USERSTOP					=8	,	    //用户停止
        BSL_ERR_UNKNOW						=9	,	    //不明错误
        BSL_ERR_OUTTIME						=10	,	    //超时
        BSL_ERR_NOINITIAL					=11	,	    //未初始化
        BSL_ERR_READFILE					=12	,	    //读文件错误
        BSL_ERR_OWRWNDNULL					=13	,	    //窗口为空
        BSL_ERR_NOFINDFONT					=14	,	    //找不到指定名称的字体
        BSL_ERR_PENNO						=15	,	    //错误的笔号
        BSL_ERR_NOTTEXT						=16	,	    //指定名称的对象不是文本对象
        BSL_ERR_SAVEFILE					=17	,	    //保存文件失败
        BSL_ERR_NOFINDENT					=18	,	    //找不到指定对象
        BSL_ERR_STATUE						=19	,	    //当前状态下不能执行此操作
        BSL_ERR_LOADNEWFILE					=20	,	    //加载振镜校正文件失败
        BSL_ERR_INCORRECTCALPOINT			=21	,	    //检定点数不正确，必须超过3x3点
        BSL_ERR_INCORRECTFILELINE			=22	,	    //文件行数不对
        BSL_ERR_OPENVEC_FAIL = 100,                     //打开向量文件失败
    }
    // public static const int MAX_LENGTH;
    class BSL_DEFINE
    {
        public const int MAX_SHAPE_COUNT = 256; //最大图元数量
        public const int BSL_BUFFER_SIZE = 256;//缓冲区大小
        public const double PI = 3.1415926;     //圆周率
        public const int MAX_SHAPE_FILL_COUNT = 4; //最大支持四层填充
        //public const int MAX_SHAPE_LINE_COUNT = 1000; //最大支持路径行数
        //public const int MAX_SHAPE_POINT_COUNT = 5000; //每行最大支持点数
        public const int MAX_SHAPE_LINE_COUNT = 500; //最大支持路径行数
        public const int MAX_SHAPE_POINT_COUNT = 500; //每行最大支持点数
    }
    public enum BARCODETYPE
    {
        BARCODETYPE_39     = 0,
        BARCODETYPE_93     = 1,
        BARCODETYPE_128A   = 2,
        BARCODETYPE_128B   = 3,
        BARCODETYPE_128C   = 4,
        BARCODETYPE_128OPT = 5,
        BARCODETYPE_EAN128A= 6,
        BARCODETYPE_EAN128B= 7,
        BARCODETYPE_EAN128C= 8,
        BARCODETYPE_EAN13  = 9,
        BARCODETYPE_EAN8   = 10,
        BARCODETYPE_UPCA   = 11,
        BARCODETYPE_UPCE   = 12,
        BARCODETYPE_25     = 13,
        BARCODETYPE_INTER25= 14,
        BARCODETYPE_CODABAR= 15,
        BARCODETYPE_PDF417 = 16,
        BARCODETYPE_DATAMTX = 17,
        BARCODETYPE_USERDEF=  18
    }

    public enum BARCODEATTRIB
    {
        BARCODEATTRIB_REVERSE          =0x0008, //条码反转
        BARCODEATTRIB_HUMANREAD        =0x1000, //显示人识别字符
        BARCODEATTRIB_CHECKNUM         =0x0004, //需要校验码
        BARCODEATTRIB_PDF417_SHORTMODE =0x0040, //PDF417为缩短模式
        BARCODEATTRIB_DATAMTX_DOTMODE  =0x0080, //DataMtrix为点模式
        BARCODEATTRIB_CIRCLEMODE       =0x0100 //自定义二维码为圆模式
    }

    public enum DATAMTX_SIZEMODE
    {
        DATAMTX_SIZEMODE_SMALLEST =0,
        DATAMTX_SIZEMODE_10X10    =1,
        DATAMTX_SIZEMODE_12X12    =2,
        DATAMTX_SIZEMODE_14X14    =3,
        DATAMTX_SIZEMODE_16X16    =4,
        DATAMTX_SIZEMODE_18X18    =5,
        DATAMTX_SIZEMODE_20X20    =6,
        DATAMTX_SIZEMODE_22X22    =7,
        DATAMTX_SIZEMODE_24X24    =8,
        DATAMTX_SIZEMODE_26X26    =9,
        DATAMTX_SIZEMODE_32X32    =10,
        DATAMTX_SIZEMODE_36X36    =11,
        DATAMTX_SIZEMODE_40X40   =12,
        DATAMTX_SIZEMODE_44X44   =13,
        DATAMTX_SIZEMODE_48X48   =14,
        DATAMTX_SIZEMODE_52X52   =15,
        DATAMTX_SIZEMODE_64X64   =16,
        DATAMTX_SIZEMODE_72X72   = 17,
        DATAMTX_SIZEMODE_80X80   = 18,
        DATAMTX_SIZEMODE_88X88   = 19,
        DATAMTX_SIZEMODE_96X96   = 20,
        DATAMTX_SIZEMODE_104X104 = 21,
        DATAMTX_SIZEMODE_120X120 = 22,
        DATAMTX_SIZEMODE_132X132 =  23,
        DATAMTX_SIZEMODE_144X144 =24,
        DATAMTX_SIZEMODE_8X18    =5,
        DATAMTX_SIZEMODE_8X32    =6,
        DATAMTX_SIZEMODE_12X26   =27,
        DATAMTX_SIZEMODE_12X36   =28,
        DATAMTX_SIZEMODE_16X36   =29,
        DATAMTX_SIZEMODE_16X48   =30
    }

    //填充类型
    public enum BSL_FILLTYPE{
	    BSL_FT_CIRCULAR = 0,		/* 环形填充 */
	    BSL_FT_SINGLE_LINE,			/* 弓形, 在单个连通区是不断开 */
	    BSL_FT_SINGLE_LINE_BREAK,	/* 弓形，跨越两个不相连的图块时，中间会断开*/
	    BSL_FT_MULTI_LINE,			/* 多线，单向填充，各线段在两端不相连*/
	    BSL_FT_MULTI_LINE_TWO_DIR	/* 多线，双向填充，各线段在两端不相连，可减少空跳时间*/
    };

    public enum BLS_FONTTYPE
    {
        FT_TRUETYPE = 0,
        FT_SINGLELINE,
        FT_CODEBAR,
        FT_PTMATRIC//,      //点阵字体 
    };
    #endregion

#region 通用结构体

    //参数
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IPandHost
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] szIp;                                      //板卡ip地址
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] szMask;                                    //子码
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] szGate;                                    //掩码
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] szCardMac;                                 //板卡mac地址,以此判断是否同一张卡
        public byte szData;                                       //无用数据
        public byte bHaveBind;                                     //该网口卡是否已经绑定
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] szHostAddr;                                //管控电脑IP
        //上面是数据包中包含的字段
        //下面是计算出来的字段
        public bool m_bSameIp;                                     //是否受本机管控
        public bool m_bLocalNet;                                   //是否和本机同网段
        public bool m_bNewProtocol;                                //是否采用新协议(目前: usb卡 旧协议  网口卡有新协议和旧协议)

    };

    //图元填充参数
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct BSL_ShapeFillPara
    {
        public BSL_FILLTYPE m_nFillType;	//填充类型, 
        public int m_nExecuteType;		//多个图形的运算方式，0=异或 1=交  2=并 3=差，
        public int m_nIndex;			//第几个填充
        public bool m_bEnable;			//使能当前填充
        public bool m_bRoundInvert;

        public double m_fLineSpacing;	//线间距
        public double m_fMargin;		// 边距

        public bool m_fill_rotate;		//自动旋转角度标刻
        public double m_fRotateAngle;	//旋转角度

        public double m_fstrFillGraduallyMaxs;	//渐变区域填充线最大间距
        public float m_fstrFillGraduallyRange;		//渐变区域设置填充宽度
        public double m_fstrFillGraduallyRate; //渐变区域变化幅度
        public float m_fstrFillGraduallyActualRange; //渐变区域实际宽度
        public bool m_bEnableFillGradually;		//使能渐变填充

	    // 以下属性对环形填充无效的
        public bool m_bWholeConsider;  /* 整体计算，当环形填充时无效*/
        public bool m_bAlongBorder; /* 绕边走一次，当环形填充时无效*/
        public bool m_bCrossFill; //交叉填充, by tgf 20180410
        public bool m_bQuickFill; //快速填充(用于单线单向与单线双向), by tgf 20180702
        public double m_fAngle;  // 填充线旋转的角度(弧度值)，对环形无效
        public UInt32 m_nFillMarkCount;// 填充线标刻次数
        public int m_nPenNum;      //笔号
        public UInt32 m_cPenColor; //颜色
        public UInt32 m_nCircularCount; // 边界环数，是除绕边走一圈以外的的环,对环形填充无效
        public double m_fCircularGap;  // 环间距，
        public double m_fInnerSpacing;   //直线缩进，是环与绕边走一圈里面的
	    // 注意：环与绕边走一圈的区别：绕边走一圈与填充线条是没有间距的。	
        public bool m_bArrangeEqually;	//平均分布各条，如果为false,则以下属性有效
        public double m_fStartPreserve;	// 开始保留
        public double m_fEndPreserve;		//结束保留

	    public void init()
	    {
		    m_bEnable = false;
		    m_nFillType = BSL_FILLTYPE.BSL_FT_CIRCULAR;
		    m_nExecuteType = 0;
		    m_bWholeConsider = true;
		    m_bAlongBorder = false;
		    m_bCrossFill = false;
		    m_bQuickFill = false;
		    m_fAngle= 0;
		    m_nFillMarkCount = 1;
		    m_nPenNum = 0;
		    m_fLineSpacing = 0.06;
		    m_bArrangeEqually = false;
		    m_fMargin = 0;
		    m_fStartPreserve= 0 ;
		    m_fEndPreserve=0;
		    m_fInnerSpacing=0;
		    m_nCircularCount = 0;
		    m_fCircularGap = 0.5;
		    m_bRoundInvert = false;
            m_cPenColor = 0;         // RGB(0, 0, 0);
		    m_fill_rotate= false;
		    m_fRotateAngle = 0;
		    m_fstrFillGraduallyMaxs  = 0.06;
		    m_fstrFillGraduallyRange = 6;
		    m_fstrFillGraduallyRate = 0.006;
		    m_fstrFillGraduallyActualRange = 6;
	    }		
    };

    //填充参数
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct BSL_FillPara
    {
	    /*使能轮廓和/或绕边走一次：
	     * 0:无轮廓，无绕边走一次；1:有轮廓无绕边；2：无轮廓，有绕边；3:有轮廓有绕边
	     * 当有轮廓且优先轮廓时，轮廓在填充线前，若不优先轮廓，则轮廓在填充线后；
	     * 绕边一次的标刻，总是在填充之后。
	     */
        public int m_bOutLine;		
	    public bool m_bOutLineFirst;	//是否先标刻轮廓
        public bool m_bKeepSeperate;   //保持填充对象的独立
        public bool m_bDelUngroup;		//删除填充时是否解散群组
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.MAX_SHAPE_FILL_COUNT)]
	    public BSL_ShapeFillPara[] m_arrPar;   //多组参数
	    public void init()
	    {
		    m_bOutLine = 1;
		    m_bOutLineFirst = true;
		    m_bKeepSeperate = false;
		    m_bDelUngroup = true;
		    bool bFirstParEnabled = true;

            m_arrPar = new BSL_ShapeFillPara[BSL_DEFINE.MAX_SHAPE_FILL_COUNT];
		    for (int i = 0; i < BSL_DEFINE.MAX_SHAPE_FILL_COUNT; i++)
		    {
                m_arrPar[i].init();
			    m_arrPar[i].m_bEnable = bFirstParEnabled;
			    m_arrPar[i].m_nPenNum = 0;
			    m_arrPar[i].m_nIndex = i;
			    bFirstParEnabled = false;
		    }
	    }
    };

    //设备ID
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct STU_DEVID
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public byte[] wszDevName;
    };

//////////////////////////////////////////////////////////////////////
  //图元信息
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct ShapeInfo2
    {
        public UInt32 nShapeIndex;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
        public byte[] wszShapeName;
        public int iShapeType;
    };
    //图元名称
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct ShapeName
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
        public byte[] wszShapeName;
    };
    //参数
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct STU_PARA
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public byte[] wszParaName;
    };
        


    //二维码类型
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct STU_BARCODETYPE
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public byte[] wszParaName;
    };


    //图元骨架信息
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct EntityInfoCSharp
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
        public byte[] wszDocname;//文档名称，图元所属文档
        public int iIndex;	//在内存库中位置索引
        public int iType;		//图元类型
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
        public byte[] wszName;//图元名称
        //图元外接矩形的图纸坐标
        public double x;		//图元左顶点x坐标（mm）
        public double y;		//图元左顶点y坐标（mm）
        public double width;	//图元宽度（mm）
        public double height;	//图元高度（mm）
    };


    //图元骨架信息
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct EntityInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
        public byte[] wszDocname;//文档名称，图元所属文档
        public int iIndex;	//在内存库中位置索引
        public int iType;		//图元类型
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
        public byte[] wszName;//图元名称
        //图元外接矩形的图纸坐标
        public double x;		//图元左顶点x坐标（mm）
        public double y;		//图元左顶点y坐标（mm）
        public double width;	//图元宽度（mm）
        public double height;	//图元高度（mm）
    };

    //区域分组图元信息
    //分组不能跨越文档
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct AreaEntityGroupCSharp
    {
        //区域范围
        public double x;		//图元左顶点x坐标（mm）
        public double y;		//图元左顶点y坐标（mm）
        public double width;	//图元宽度（mm）
        public double height;	//图元高度（mm）

        public int nCount;		//分组内图元数量
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public EntityInfo[] pEntity;//分组内图元
    };


    //区域分组图元信息
    //分组不能跨越文档
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct BlockPosPathCShape
    {
        //区域范围
        public double x;		//图元左顶点x坐标（mm）
        public double y;		//图元左顶点y坐标（mm）
        public double width;	//图元宽度（mm）
        public double height;	//图元高度（mm）
    };


    //点
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct POINTF
    {
        public float x;
        public float y;
    };


    //路径点数据
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct LineDataShape
    {
        public int nPtCount;    //行数   点数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.MAX_SHAPE_POINT_COUNT)]
        public POINTF[] pPoint;   //点

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.nPtCount = 0;

            for (int i = 0; i < BSL_DEFINE.MAX_SHAPE_POINT_COUNT; i++)
            {
                pPoint[i].x = 0.0F;
                pPoint[i].y = 0.0F;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nCnt">点阵数</param>
        public LineDataShape(int nCnt)
        {
            this.nPtCount = nCnt;

            pPoint = new POINTF[BSL_DEFINE.MAX_SHAPE_POINT_COUNT];

            Init();
        }
    };

    //路径数据
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PathDataShape
    {
        public int nPenIdx;

        public int nMarkCount;

        public int nLineCount;    //行数   路径数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.MAX_SHAPE_LINE_COUNT)]
        public LineDataShape[] pLine;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            nLineCount = 0;

            for (int i = 0; i < BSL_DEFINE.MAX_SHAPE_LINE_COUNT; i++)
            {
                pLine[i].Init();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nCnt">行数</param>
        public PathDataShape(int nCnt)
        {
            nLineCount = nCnt;
            this.nPenIdx = 0;
            this.nMarkCount = 0;

            pLine = new LineDataShape[BSL_DEFINE.MAX_SHAPE_LINE_COUNT];
            for (int i = 0; i < BSL_DEFINE.MAX_SHAPE_LINE_COUNT; i++)
            {
                pLine[i] = new LineDataShape(0);
            }
            Init();
        }
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    //网格检定数据
    public struct gridCalData
    {
        public double xT;	//理论点X
        public double yT;	//理论点Y
        public double xA;	//实测点X
        public double yA;	//实测点Y
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(gridCalData src)
        {
            xT = src.xT;
            yT = src.yT;
            xA = src.xA;
            yA = src.yA;
        }
        ///<summary>
        ///构造函数
        ///</summary>
        ///<param name="nCnt">行数</param>
        public gridCalData(gridCalData src)
        {
            xT = 0.0F;
            yT = 0.0F;
            xA = 0.0F;
            yA = 0.0F;
            Init(src);
        }

    };

    //GCODE
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct STU_GCODE
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
        public byte[] wszGCode;
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    //配合微伦增加标刻关联文件的结构体 Auth：LYF 2023.6.25
    public struct Offset
    {
        public double offsetX;
        public double offsetY;
        public double rotCenterX;
        public double rotCenterY;
        public double rotAngle;
    }

    #endregion

#region 设备
    //初始化全部USB设备
    //hOwrWnd 指拥有用户输入焦点的窗口，用于检测用户暂停消息
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_InitDevices(Int32 dwWndHanle);

    //重置全部USB设备，将重新初始化所有的USB设备
    //hOwrWnd 指拥有用户输入焦点的窗口，用于检测用户暂停消息
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ResetDevices(Int32 dwWndHanle);

    //重新初始化全部USB设备(拔插设备时调用)
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ReInitDevices([MarshalAs(UnmanagedType.LPWStr)] string strDevID, bool bAdd);

    //关闭所有的USB设备
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_CloseAllDevice();
#pragma region  网口卡设备初始化
    /**  
    *初始化网口卡设备(UDP)
    * 输入参数： hWnd------主窗口的句柄
                 Msg-------回传信息，触发设备更新
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_UDPInit(IntPtr dwWndHanle, int Msg);

        //注册网口卡设备(UDP)的插拔消息
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_OnUDPDeviceChanged(IntPtr wParam, IntPtr lParam);

        /** UDP反初始化
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_UDPUnInit();

        #pragma region  
            //设置是否查找新的UDP设备(当设备都查找到以后，可以设为非查找模式,这样更稳定)
            [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
            public delegate BslErrCode BSL_UDPSetFindMode(bool bFind);

            /** 获取所有的网口设备信息
            */
            [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
            public delegate BslErrCode BSL_UDPGetAllDevsInfo([Out]IPandHost[] vAllDevsInfo, ref int nDevCount);


            /** 获取所有的与本机已经绑定的网口设备
                */
            [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
            public delegate BslErrCode BSL_UDPGetBindDevsInfo([Out]IPandHost[] vBindDevsInfo, ref int nDevCount);

            /** 绑定某个设备
              * 输入参数：vecClientInfo ---- 所有设备信息，可以通过UDPGetAllDevsInfo获取
              wstrIP -----需要绑定网口设备的IP
              */
            [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
            public delegate BslErrCode BSL_BindIP([MarshalAs(UnmanagedType.LPWStr)] string strDevIP);

            /**从所有设备中解绑某个设备
            * 输入参数：vecClientInfo ---- 所有设备信息，可以通过UDPGetAllDevsInfo获取
            wstrIP -----需要绑定网口设备的IP
            */
            [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
            public delegate BslErrCode BSL_UnBindIP([MarshalAs(UnmanagedType.LPWStr)] string strDevIP);

            /**从绑定的列表中删除某个设备
            * 输入参数：
                wstrIP -----需要绑定网口设备的IP
            */
            [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
            public delegate BslErrCode BSL_UDPDeleBindDevsInfo([MarshalAs(UnmanagedType.LPWStr)] string strDevIP);
#pragma region 
#pragma endregion  网口卡设备初始化

    //获取所有的设备ID
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetAllDevices2([Out] STU_DEVID[] vDevID, ref int nDevCount);

    #endregion

#region 参数

    //重新加载配置文件
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_ReLoadSeaPar();

    //获取所有参数
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetAllPara2([Out] STU_PARA[] vDevID, ref int nParCount);

    //复制一份参数至参数库
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_CopyPara([MarshalAs(UnmanagedType.LPWStr)] string strParName, [MarshalAs(UnmanagedType.LPWStr)] string NewParName);

    //获取关联设备的参数
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetAssocParaNameByDevID([MarshalAs(UnmanagedType.LPWStr)] string strDevID, char[] strParName);

    //关联设备与参数
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AssocDevPara([MarshalAs(UnmanagedType.LPWStr)] string strDevID, [MarshalAs(UnmanagedType.LPWStr)] string strParName);

    //关联设备与参数
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_DisplayDevCfgDlg([MarshalAs(UnmanagedType.LPWStr)] string strDevID);
    #region 笔号

    //得到指定笔号对应的加工参数,文件名称设置为NULL表示获取全局的笔号
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetPenParam([MarshalAs(UnmanagedType.LPWStr)] string strFileName,  //文件名称
            UInt32 nPenNo,                     //要读取的笔号(0-255)	
            ref int nMarkLoop,              //加工次数
            ref double dMarkSpeed,          //标刻次数mm/s
            ref double dPowerRatio,         //功率百分比(0-100%)	
            ref double dCurrent,            //电流A
            ref float fFreq,                //频率KHZ
            ref float fQPulseWidth,           //Q脉冲宽度us	
            ref int nStartTC,               //开始延时us
            ref int nLaserOffTC,            //激光关闭延时us
            ref int nEndTC,                 //结束延时us
            ref int nPolyTC,                //拐角延时us
            ref double dJumpSpeed,          //跳转速度mm/s
            ref int nJumpPosTC,             //跳转位置延时us 
            ref int nJumpDistTC,            //跳转距离延时us	
            ref double dEndComp,            //末点补偿mm				
            ref bool bPulsePointMode,       //脉冲点模式 
            ref int nPulseNum,              //脉冲点数目
            ref float POINTTIME);           // 打点时间

    //设置指定笔号对应的加工参数,文件名称设置为NULL表示设置全局的笔号，影响出点光，标刻直线等。
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SetPenParam([MarshalAs(UnmanagedType.LPWStr)] string strFileName, //文件名称
            UInt32 nPenNo,                  //要设置的笔号(0-255)	0
            int nMarkLoop,              //加工次数                1
            double dMarkSpeed,          //标刻次数mm/s            1000
            double dPowerRatio,         //功率百分比(0-100%)	     75
            double dCurrent,            //电流A                   1
            float fFreq,                  //频率KHZ               30
            float fQPulseWidth,        //Q脉冲宽度us	             10
            int nStartTC,               //开始延时us               1
            int nLaserOffTC,            //激光关闭延时us            100
            int nEndTC,                 //结束延时us                50
            int nPolyTC,                //拐角延时us                80
            double dJumpSpeed,          //跳转速度mm/s              4000
            int nJumpPosTC,             //跳转位置延时us             500
            int nJumpDistTC,            //跳转距离延时us	           100
            double dEndComp,            //末点补偿mm				   0.2
            bool bPulsePointMode,       //脉冲点模式                 true
            int nPulseNum,              //脉冲点数目                 0
            float POINTTIME);           // 打点时间                 100

    //得到指定笔号对应的加工参数,文件名称设置为NULL表示获取全局的笔号
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetPenParam2([MarshalAs(UnmanagedType.LPWStr)] string strFileName,  //文件名称
            UInt32 nPenNo,                     //要读取的笔号(0-255)	
            ref int nMarkLoop,              //加工次数
            ref double dMarkSpeed,          //标刻次数mm/s
            ref double dPowerRatio,         //功率百分比(0-100%)	
            ref double dCurrent,            //电流A
            ref float nFreq,                  //频率KHZ
            ref float fQPulseWidth,           //Q脉冲宽度us	
            ref int nMopaPulse,             //mopa脉冲宽度ns
            ref int nStartTC,               //开始延时us
            ref int nLaserOffTC,            //激光关闭延时us
            ref int nEndTC,                 //结束延时us
            ref int nPolyTC,                //拐角延时us
            ref double dJumpSpeed,          //跳转速度mm/s
            ref int nJumpPosTC,             //跳转位置延时us 
            ref int nJumpDistTC,            //跳转距离延时us	
            ref double dEndComp,            //末点补偿mm				
            ref bool bPulsePointMode,       //脉冲点模式 
            ref int nPulseNum,              //脉冲点数目
            ref float POINTTIME);           // 打点时间

    //设置指定笔号对应的加工参数,文件名称设置为NULL表示设置全局的笔号，影响出点光，标刻直线等。
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SetPenParam2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, //文件名称
            UInt32 nPenNo,                  //要设置的笔号(0-255)	
            int nMarkLoop,              //加工次数
            double dMarkSpeed,          //标刻次数mm/s
            double dPowerRatio,         //功率百分比(0-100%)	
            double dCurrent,            //电流A
            float fFreq,                  //频率KHZ
            float fQPulseWidth,        //Q脉冲宽度us	
            int nMopaPulse,             //mopa脉冲宽度ns
            int nStartTC,               //开始延时us
            int nLaserOffTC,            //激光关闭延时us
            int nEndTC,                 //结束延时us
            int nPolyTC,                //拐角延时us
            double dJumpSpeed,          //跳转速度mm/s
            int nJumpPosTC,             //跳转位置延时us 
            int nJumpDistTC,            //跳转距离延时us	
            double dEndComp,            //末点补偿mm				
            bool bPulsePointMode,       //脉冲点模式 
            int nPulseNum,              //脉冲点数目
            float POINTTIME);           // 打点时间


    //设置对象笔号
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SetShapePenNo([MarshalAs(UnmanagedType.LPWStr)] string strFileName, /*打开的文件*/
                    [MarshalAs(UnmanagedType.LPWStr)] string szEntName,  /*对象名称*/
                    UInt32 nPenNo); /*笔号*/

    //设置对象笔号
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SetShapePenNoByEntName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, /*打开的文件*/
                    [MarshalAs(UnmanagedType.LPWStr)] string szEntName,  /*对象名称*/
                    UInt32 nPenNo); /*笔号*/
    
    //获取对象笔号
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetShapePenNoByEntName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, /*打开的文件*/
                    [MarshalAs(UnmanagedType.LPWStr)] string szEntName,  /*对象名称*/
                    ref UInt32 nPenNo); /*笔号*/
    #endregion
    #endregion

#region  文档
 
    //加载图形文件
    //输入参数: strFileName  图形文件名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_LoadDataFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName);


    //新建一个orzx文档文件
    //输入参数: strFileName  图形文件名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_NewFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName);


    //卸载图形文件
    //输入参数: strFileName  图形文件名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_UnLoadDataFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName);

    #region  图形显示
        //预览图形文件 供c#调用
        //输入参数: strFileName  图形文件名称
        //			hWnd 图形显示的窗口
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_DrawFileInWnd2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 dwWndHanle, bool bDrawAxis);

        //预览图形文件 供c#调用
        //输入参数: strFileName  图形文件名称
        //			hWnd 图形显示的窗口
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate IntPtr BSL_DrawFileInImg([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 dwWidth, Int32 dwHeight, bool bDrawAxis);


        //预览图形文件-----bDrawGlvArea为：true最大显示振镜区域，false为最大图形显示。并支持缩放
        //输入参数: strFileName  图形文件名称
        //			nWidth nHeight 图形显示的范围大小
        //          fDelta 放缩比，调节放缩的尺寸
        //          bDrawGlvArea  是否显示振镜区域线
        //			bDrawAxis   是否绘制轴线 
        //			nBlankSize  两边留白像素点
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate IntPtr BSL_DrawFileInImgEx([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 dwWidth, Int32 dwHeight, float fDelta, bool bDrawGlvArea, bool bDrawAxis, UInt32 nBlankSize, bool bDrawSelectRect);

        //预览图形文件-----bDrawGlvArea为：true最大显示振镜区域，false为最大图形显示。并支持缩放
        //输入参数: strFileName  图形文件名称
        //			nWidth nHeight 图形显示的范围大小
        //          fDelta 放缩比，调节放缩的尺寸
        //          bDrawGlvArea  是否显示振镜区域线
        //			bDrawAxis   是否绘制轴线 
        //			nBlankSize  两边留白像素点
        //          fDrawOffsetX 、fDrawOffsetY 鼠标移动的设备坐标值
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate IntPtr BSL_DrawFileInImgEx2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 dwWidth, Int32 dwHeight, float fDelta, bool bDrawGlvArea, bool bDrawAxis, UInt32 nBlankSize, bool bDrawSelectRect, float fDrawOffsetX, float fDrawOffsetY);

        //预览图形文件-----bDrawGlvArea为：true最大显示振镜区域，false为最大图形显示。并支持缩放
        //输入参数: strFileName  图形文件名称
        //          strEntName   图元名称
        //			nWidth nHeight 图形显示的范围大小
        //          fDelta 放缩比，调节放缩的尺寸
        //          bDrawGlvArea  是否显示振镜区域线
        //			bDrawAxis   是否绘制轴线 
        //			nBlankSize  两边留白像素点
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate IntPtr BSL_GetPrevBitmapByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, Int32 dwWidth, Int32 dwHeight, float fDelta, bool bDrawGlvArea, bool bDrawAxis, UInt32 nBlankSize, bool bDrawSelectRect);


        //预览图形文件-----bDrawGlvArea为：true最大显示振镜区域，false为最大图形显示。并支持缩放
        //输入参数: strFileName  图形文件名称
        //          iShapeIndex   图元序号
        //			nWidth nHeight 图形显示的范围大小
        //          fDelta 放缩比，调节放缩的尺寸
        //          bDrawGlvArea  是否显示振镜区域线
        //			bDrawAxis   是否绘制轴线 
        //			nBlankSize  两边留白像素点
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate IntPtr BSL_GetPrevBitmapByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 iShapeIndex, Int32 dwWidth, Int32 dwHeight, float fDelta, bool bDrawGlvArea, bool bDrawAxis, UInt32 nBlankSize, bool bDrawSelectRect);

    #endregion   图形显示

    //保存当前数据库里所有对象到指定图形文件里
    //输入参数: strFileName 图形文件名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SaveEntLibToFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName);

    //当前数据库里所有对象另保存到指定图形文件里
    //输入参数: strFileName 图形文件名称
    //输入参数: strSaveAsFileName 另存为图形文件名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SaveAsEntLibToFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strSaveAsFileName);

    //将图形文件附加到设备
    //输入参数: strFileName  图形文件名称
    //			strDevID 设备ID
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AppendFileToDevice([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strDevID);

    //将图形文件从设备解除关联
    //输入参数: strFileName  图形文件名称
    //			strDevID 设备ID
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_UnappendFileFromDevice([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strDevID);

    //解除指定设备与所有已关联文档的关联
    //输入参数: szDevID: 设备ID
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate int BSL_DeAllDocFromDev([MarshalAs(UnmanagedType.LPWStr)] string strDevID);

    public struct STU_FILENAME
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
        public byte[] wszParaName;
    };
    //获取设备的关联图形文件列表
    //输入参数: vFileName  图形文件名称列表
    //			strDevID 设备ID
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetAssocFilesOfDevice2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [Out] STU_FILENAME[] vFileName, ref int nNameCount);

    //获取某个打开的文件中的图形列表
    //输入参数: strFileName  图形文件名称
    //			vShapes 图形信息容器
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetShapesInFile2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [Out] ShapeInfo2[] shapes, int nMaxShapeSize);

    //得到对象总数
    //输出参数:  对象总数
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetEntityCount([MarshalAs(UnmanagedType.LPWStr)] string strFileName);

    // 移动图元在列表中位置
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MoveEntityOrderByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 iIndex, Int32 iOrderOffset);

    // 反向图元在列表中位置
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ReverseAllEntOrder([MarshalAs(UnmanagedType.LPWStr)] string strFileName);
    #endregion   文档

#region  图形绘制

    //加入一组点到数据库中
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AddPointToFile(
     [MarshalAs(UnmanagedType.LPWStr)] string szFileName,//目标文件名
     IntPtr ptBuf,//点数组
     int ptNum,//点数
     [MarshalAs(UnmanagedType.LPWStr)] string pEntName,//直线段对象名称
     int nPenNo); //直线对象使用的笔号

    //加入多直线段到数据库中
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AddLinesToFile(
    [MarshalAs(UnmanagedType.LPWStr)] string szFileName,//目标文件名
    IntPtr ptBuf,//直线段顶点数组
    int ptNum,//直线段顶点数
    [MarshalAs(UnmanagedType.LPWStr)] string pEntName,//直线段对象名称
    int nPenNo); //直线对象使用的笔号

    //加入多直线段到数据库中
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AddLinesToFile2(
    [MarshalAs(UnmanagedType.LPWStr)] string szFileName,//增加到的目标文件名，增加后仍然需要手动保存才能保存到文件。
    POINTF[] vPoints, //所有点构成的数组
    int iLineCount,//独立线的总数量（非连接在一起的线段数量）
    int[] iPtCount,//独立线的分隔点数构成的数组
    [MarshalAs(UnmanagedType.LPWStr)] string pEntName,//直线段对象名称
    int nPenNo); //激光加工笔号

    //加入新圆到数据库中
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AddCircleToFile(
     [MarshalAs(UnmanagedType.LPWStr)] string szFileName,//圆增加到的目标文件名，增加后仍然需要手动保存才能保存到文件。
     [MarshalAs(UnmanagedType.LPWStr)] string pEntName,//圆对象名称
     double dPosX,//圆中心点的x坐标
     double dPosY,//圆中心点的y坐标
     double dPosZ,//圆对象的z坐标
     double dRadius,//圆半径
     double dRotateAngle,//绕中心点旋转的角度值(弧度值)
     int nPenNo,//对象使用的加工参数
     bool bFill //是否填充对象
     );

    //加入新椭圆到数据库中
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AddEllipseToFile(
    [MarshalAs(UnmanagedType.LPWStr)] string szFileName,//圆增加到的目标文件名，增加后仍然需要手动保存才能保存到文件。
    [MarshalAs(UnmanagedType.LPWStr)] string pEntName,//圆对象名称
    double dPosX,//圆中心点的x坐标
    double dPosY,//圆中心点的y坐标
    double dPosZ,//圆对象的z坐标
    double dLongAxis,//椭圆长轴
    double dMinorAxis,//椭圆短轴
    double dRotateAngle,//绕中心点旋转的角度值(弧度值)
    int nPenNo,//对象使用的加工参数
    bool bFill //是否填充对象
    );

    //对齐方式时数字代表的意义:0是左对齐，1是中对齐，2是右对齐
    //   0 ---  1 --- 2
    //加入新文本到数据库中
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AddTextToFile(
    [MarshalAs(UnmanagedType.LPWStr)] String strFileName, // 文本增加到的目标文件名，增加后仍然需要手动保存才能保存到文件。
    [MarshalAs(UnmanagedType.LPWStr)] String pStr,   //要添加的字符串
    [MarshalAs(UnmanagedType.LPWStr)] String pEntName,  //字符串对象名称
    double dPosX,//字符串的左下角基点的x坐标
    double dPosY,//字符串的左下角基点的y坐标
    double dPosZ,//字符串对象的z坐标
    int nAlign,//对齐方式0－8
    double dTextRotateAngle,//字符串绕基点旋转的角度值(弧度值)
    int nPenNo,//对象使用的加工参数
    bool bHatchText);//是否填充文本对象

    //加入新文本到数据库中
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AddTextToFileEx(
    [MarshalAs(UnmanagedType.LPWStr)] String strFileName, // 文本增加到的目标文件名，增加后仍然需要手动保存才能保存到文件。
    [MarshalAs(UnmanagedType.LPWStr)] String pStr,   //要添加的字符串
    [MarshalAs(UnmanagedType.LPWStr)] String pEntName,  //字符串对象名称
    double dPosX,//字符串的左下角基点的x坐标
    double dPosY,//字符串的左下角基点的y坐标
    double dPosZ,//字符串对象的z坐标
    int nAlign,//对齐方式0－2
    double dTextRotateAngle,//字符串绕基点旋转的角度值(弧度值)
    int nPenNo,//对象使用的加工参数
    bool bHatchText,//是否填充文本对象
    double dHeight,  //字体高度
    [MarshalAs(UnmanagedType.LPWStr)] string pTextFontName);  //字体名称

    //加入条码到数据库中
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AddBarcodeToFile(
    [MarshalAs(UnmanagedType.LPWStr)] string strFileName,
    [MarshalAs(UnmanagedType.LPWStr)] string pStr,   //要添加的字符串
    [MarshalAs(UnmanagedType.LPWStr)] string pEntName,  //字符串对象名称
    double dPosX, //字符左下角基点x坐标
    double dPosY, //字符左下角基点y坐标
    double dPosZ, //字符z坐标
    int nAlign,//对齐方式0－8
    int nPenNo,					 
    int nRow,//二维码行数
    int nCol,//二维码列数
    int nCheckLevel,//pdf417错误纠正级别0-8
    int nSizeMode,//DataMatrix尺寸模式0-30
    [MarshalAs(UnmanagedType.LPWStr)] string pTextFontName
    );

    //加载矢量文件
    //输入参数: strFileName  矢量文件名称，也是生成的图形文件的路径
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_LoadVectorFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName, bool bIsCenter);

    //加载矢量文件
    //输入参数: szFileName         图形文件名称
    //          szVectorFileName   矢量文件名称
    //          szEntName          图元名称
    //          bIsCenter           是否居中
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_LoadVectorFileEx([MarshalAs(UnmanagedType.LPWStr)] string szFileName, [MarshalAs(UnmanagedType.LPWStr)] string szVectorFileName, [MarshalAs(UnmanagedType.LPWStr)] string szEntName, bool bIsCenter);

    // 替换矢量文件
    // 输入参数: strFileName 文件名
    // 输入参数: strVectorFileName 矢量文件名
    // 输入参数: strEntName 图元实体名
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate int BSL_ReplaceVectorFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strVectorFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName);

    //加入图片到数据库中
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AddBmpToFile(
    [MarshalAs(UnmanagedType.LPWStr)] string strFileName,   //模板(orzx)文件名称
    [MarshalAs(UnmanagedType.LPWStr)] string sBmpFileName,		//要添加图片的路径,支持gif,jpg,jpeg,png,bmp,tif.tiff,emf格式图片
    [MarshalAs(UnmanagedType.LPWStr)] string pEntName,      //字符串对象名称(为NULL则自动截取文件名 非NULL由外部传入)
    double W,                //尺寸X
    double H,                //尺寸Y
    double dPosX,            //中心点的x坐标
    double dPosY,            //中心点的y坐标
    double dRotateAngle,     //绕基点旋转的角度值(弧度值)
    int nPenNo               //对象使用的加工参数
    );

    //通过下标替换图片
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate int BSL_ReplacePictureByIndex(
    [MarshalAs(UnmanagedType.LPWStr)] String szFileName,            //原图片所在文件名
            int nIndex,                                             //原图片下标
    [MarshalAs(UnmanagedType.LPWStr)] String szNewPicturePath);  //新图片路径

    #endregion   图形绘制

#region  图形编辑
    #region  图元对象变换
         /*
         移动指定对象相对坐标
         参数:strFileName --- 文档路径
              iIndex -------- 对象索引号
              dMovex、dMovey -- 移动的相对值
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MoveEntityRelByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, double dMovex, double dMovey);

         /*
        移动指定对象相对坐标
        参数:strFileName --------- 文档路径
             strEntName ---------- 对象名称
             dMovex、dMovey ------ 移动的相对值
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MoveEntityRelByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, double dMovex, double dMovey);

        /*
         移动指定对象绝对坐标
         参数:strFileName --------- 文档路径
              iIndex -------------- 对象索引号
              dPtx、dPty ---------- 绝对坐标点
         */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MoveEntityAbsByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, double dPtx, double dPty);

        /*
         移动指定对象绝对坐标
         参数:strFileName ------- 文档路径
              strEntName -------- 对象名称
              dPtx、dPty -------- 绝对坐标点
         */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_MoveEntityAbsByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, double dPtx, double dPty);


        //缩放指定对象，缩放中心坐标(dCenx，dCeny)  dScaleX=X方向缩放比例  dScaleY=Y方向缩放比例
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ScaleEntityByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName,
        int iIndex,//对象索引号
        double dCenx,
        double dCeny,
        double dScaleX,
        double dScaleY);

        //缩放指定对象，缩放中心坐标(dCenx，dCeny)  dScaleX=X方向缩放比例  dScaleY=Y方向缩放比例
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ScaleEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName,
        [MarshalAs(UnmanagedType.LPWStr)] string strEntName,//对象名称
        double dCenx,
        double dCeny,
        double dScaleX,
        double dScaleY);

        //旋转指定对象  
        //iIndex对象索引号
        //(dCenx，dCeny) 旋转中心坐标
        //dAngle=旋转角度(逆时针为正，单位为度)  
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_RotateEntityByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, double dCenx, double dCeny, double dAngle);

        //旋转指定对象  
        //pEntName 对象名称
        //(dCenx，dCeny) 旋转中心坐标
        //dAngle = 旋转角度(逆时针为正，单位为度)  
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_RotateEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, double dCenx, double dCeny, double dAngle);

        //倾斜指定对象  
        //iIndex对象索引号
        //(dx，dy) 错切角度正切值
        //(dCenx，dCeny) 错切基准坐标
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SlopeEntityByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, double dCenx, double dCeny, double dx, double dy);

        //倾斜指定对象  
        //strEntName对象名称
        //(dx，dy) 错切角度正切值
        //(dCenx，dCeny) 错切基准坐标
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SlopeEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, double dCenx, double dCeny, double dx, double dy);
    #endregion   图元对象变换

    //镜像指定对象，镜像中心坐标(dCenx，dCeny)  bMirrorX=TRUE X方向镜像  bMirrorY=TRUE Y方向镜像
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MirrorEntityByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName,
    int iIndex,//对象索引号
    double dCenx,
    double dCeny,
    bool bMirrorX,
    bool bMirrorY);

    //镜像指定对象，镜像中心坐标(dCenx，dCeny)  bMirrorX=TRUE X方向镜像  bMirrorY=TRUE Y方向镜像
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MirrorEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName,
    [MarshalAs(UnmanagedType.LPWStr)] string strEntName,//对象名称
    double dCenx,
    double dCeny,
    bool bMirrorX,
    bool bMirrorY);

    // 复制选定图元
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_CopyEntByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, [MarshalAs(UnmanagedType.LPWStr)] string pNewEntName);

    // 复制选定图元
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_CopyEntByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, [MarshalAs(UnmanagedType.LPWStr)] string pNewEntName);

    // 根据index 显示 / 隐藏图元
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ShowOrHideEntityByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, bool bShow);

    //根据name 显示/隐藏图元
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ShowOrHideEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, bool bShow);

    //根据index 获取图元显隐状态
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool BSL_IsEntityVisibleByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex);

    //根据name 获取图元显隐状态
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool BSL_IsEntityVisibleByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName);

    //设置图形选中状态
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SetEntitySelectStateByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, bool bSelect);

    //设置图形选中状态
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SetEntitySelectStateByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, bool bSelect);

    //获取图形选中状态
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool BSL_GetEntitySelectStateByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex);

    //获取图形选中状态
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool BSL_GetEntitySelectStateByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName);

    //删除当前数据库里的指定文本对象
    //输入参数: strFileName		图形文件名称
    //			strEntName      要删除对象的名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_DeleteEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName);

    //删除当前数据库里的全部对象
    //输入参数: strFileName		图形文件名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_DeleteAllEntityByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName);

    //清除对象库里所有数据
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ClearEntityLib([MarshalAs(UnmanagedType.LPWStr)] String strFileName);

    //群组两个实体 将szEntName1和szEntName2 群组后命名为szEntName_New
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GroupEnt([MarshalAs(UnmanagedType.LPWStr)] String strFileName, [MarshalAs(UnmanagedType.LPWStr)] String szEntName1, [MarshalAs(UnmanagedType.LPWStr)] String szEntName2, [MarshalAs(UnmanagedType.LPWStr)] String szEntName_New);

    //解散群体
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_UnGroupEnt([MarshalAs(UnmanagedType.LPWStr)] String strFileName, [MarshalAs(UnmanagedType.LPWStr)] String szEntName);


    #region  图元填充
        // 设置填充参数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SetFillParam(ref BSL_FillPara fillPar);

        // 获取图元填充参数
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetEntFillParam([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, ref BSL_FillPara fillPar);


        /*根据轮廓线获取填充数据
	    * outline - [输入]轮廓线（可由多条路径构成）线段接点坐标
	    * outlinepnts - [输入]每条轮廓路径的接点数
	    * outlinecount - [输入]轮廓路径数量
	    * fillPar - [输入]填充参数
	    * fillLines - [输出]填充线（可由多条路径构成）线段端点坐标
	    * filllinepnts - [输出]每条填充路径的接点数
	    * filllinecount - [输出]填充路径数量
	    */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetFillData(POINTF[] outline, int[] outlinepnts, int outlinecount, BSL_FillPara fillPar, [Out] POINTF[] fillLines, ref int filllinecount, [Out] int[] filllinepnts);
       
        //填充对象
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_FillEntity([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, [MarshalAs(UnmanagedType.LPWStr)] string strEntNameNew);

        //删除填充
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_UnFillEnt([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, bool bUnGroup);

        //填充单个对象
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_FillSingleEntity([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int nShapeIndex, [MarshalAs(UnmanagedType.LPWStr)] string pEntNameNew);
        //删除单个填充
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_UnFillSingleEnt([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int nShapeIndex, bool bUnGroup);
    #endregion     图元填充


    #endregion   图形编辑

#region  图元属性
    /*
     @函数:根据图元的索引号得到指定对象的最大最小坐标
     @输出参数:
        strFileName:文档的绝对路径
        iIndex:图元对象索引号
        dMinx，dMiny:最小的坐标
        dMaxx，dMaxy:最大的坐标
        dz:Z轴坐标，暂时无用
     */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetEntSizeByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int nIndex, ref double dMinx, ref double dMiny, ref double dMaxx, ref double dMaxy, ref double dZ);

     /*
     @函数:根据图元的名称得到指定对象的最大最小坐标
     @输出参数:
       strFileName:文档的绝对路径
        strEntName:图元对象名称
        dMinx，dMiny:最小的坐标
        dMaxx，dMaxy:最大的坐标
        dz:Z轴坐标，暂时无用
     */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetEntSizeByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, ref double dMinx, ref double dMiny, ref double dMaxx, ref double dMaxy, ref double dZ);


    /*
    @函数:根据图元索引号得到指定对象的名称
	@输入参数
		strFileName:文档的绝对路径
	@输出参数:		
		nEntityIndex: 图元对象索引号
		strEntName:图元对象名称
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetEntityNameByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 nEntityIndex, [Out] ShapeName[] strEntName);

    /*
	@函数:根据图元索引号设置指定对象的名称
	@输入参数:
		strFileName:文档的绝对路径
		nEntityIndex: 图元对象索引号
		strEntName:图元对象名称
	*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SetEntityNameByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 nEntityIndex, [MarshalAs(UnmanagedType.LPWStr)] string strEntName);

    /*
      @函数:根据图元名称设置指定序号的实体名
      @输入参数:
      strFileName:文档的绝对路径
      szOldEntName: 图元对象名称
      strEntName:新的图元对象名称
      */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ChangeEntName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)] StringBuilder szOldEntName, [MarshalAs(UnmanagedType.LPWStr, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)] StringBuilder szNewEntName);

    //获取缓存中的外接矩形小于给定尺寸的图形，并且按区域全部图元骨架信息   for C#
    //输入参数：width - 宽度 mm
    //			height - 高度 mm
    //输出参数: vEntities 图元骨架信息容器
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetEntitiesBySize2([MarshalAs(UnmanagedType.LPWStr)] string szDocName, double width, double height, ref int nCount, [Out] AreaEntityGroupCSharp[] pGroup);

    //获取缓存中的全部图元骨架信息 for C#
    //输出参数: vEntities 图元骨架信息容器
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetAllEntities2([MarshalAs(UnmanagedType.LPWStr)] string szDocName, ref int nCount, [Out] EntityInfoCSharp[] pEntities);

    #region  文本图元相关属性
        /*
        @函数:设置字体参数
        @输入参数:
            strFileName : 文档的绝对路径
            strEntity   : 图元对象名称
            strFontName : 字体的类型名称，如Arial
            dCharHeight : 字高
            dCharWidth  : 字符宽度百分比（相对于字符高，在重构文本中为文本宽度缩放率，此时默认值应该是100%）
            dCharAngle  : 字符倾斜角度
            dCharSpace  : 字符间距
            dLineSpace  : 行间距
            bEqualCharWidth:是否为等宽字符
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
         public delegate BslErrCode BSL_SetTextEntParam(
         [MarshalAs(UnmanagedType.LPWStr)] string strFileName,
         [MarshalAs(UnmanagedType.LPWStr)] string strEntity,
         StringBuilder strFontName,
         double dCharHeight,
         double dCharWidth,
         double dCharAngle,
         double dCharSpace,
         double dLineSpace,
         bool bEqualCharWidth
         );

        /*
        @函数:获取字体参数
        @输出参数:
          strFileName : 文档的绝对路径
          strEntity   : 图元对象名称
          strFontName : 字体的类型名称，如Arial
          dCharHeight : 字高
          dCharWidth  : 字符宽度百分比（相对于字符高，在重构文本中为文本宽度缩放率，此时默认值应该是100%）
          dCharAngle  : 字符倾斜角度
          dCharSpace  : 字符间距
          dLineSpace  : 行间距
          bEqualCharWidth:是否为等宽字符
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetTextEntParam(
        [MarshalAs(UnmanagedType.LPWStr)] string strFileName,
        [MarshalAs(UnmanagedType.LPWStr)] string strEntity,
        StringBuilder strFontName,
        ref double dCharHeight,
        ref double dCharWidth,
        ref double dCharAngle,
        ref double dCharSpace,
        ref double dLineSpace,
        ref bool bEqualCharWidth
        );

        /*
        @函数:根据索引号改变文本图元的圆弧属性
        @输入参数:
            szFileName		: 文档的绝对路径
            nIndex			: 图元对象索引号
            bEnableArcText  : 是否启用圆弧文本
            nAlign			: 对齐方式
            fDiameter       : 圆弧直径
            fBaseAngle		: 基准角度
            bLimitAngle		: 角度范围限制
            bInvert			: 是否反转
            bUpsidedown		: 是否上下反转
            bFontInside		: 文本是否位于圆弧内侧
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ChangeTextArcPropertiesByIndex(
        [MarshalAs(UnmanagedType.LPWStr)] String szFileName, 
        int nIndex,              
        bool bEnableArcText, 
        int nAlign,              
        float fDiameter,     
        float fBaseAngle,        
        bool bLimitAngle,        
        bool bInvert,            
        bool bUpsidedown,        
        bool bFontInside);

        /*
        @函数:重置序列号
        @输入参数:
            strFileName	:文档的绝对路径
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ResetSN([MarshalAs(UnmanagedType.LPWStr)] string strFileName);

        /*
        @函数:根据图元名称判断此文本图元对象是否为变量文本
        @输入参数:
            szFileName	: 文档的绝对路径
            pEntName	: 图元对象名称
        @输出参数:
            bVarText	: 是否为变量文本
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_IsVarText([MarshalAs(UnmanagedType.LPWStr)] string szFileName, [MarshalAs(UnmanagedType.LPWStr)] string pEntName, ref bool bVarText);

        /*
        @函数:根据图元名称更改指定文本对象的文本内容
        @输入参数:
            strFileName	: 文档的绝对路径
            strTextName	: 图元对象名称
            strTextNew	: 新的文本内容
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ChangeTextByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strTextName, [MarshalAs(UnmanagedType.LPWStr)] string strTextNew);

        /*
        @函数:根据图元索引号更改指定文本对象的文本内容
        @输入参数:
            strFileName	: 文档的绝对路径
            iShapeIndex	: 图元对象索引号
            strTextNew	: 新的文本内容
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ChangeTextByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 iShapeIndex, [MarshalAs(UnmanagedType.LPWStr)] string strTextName);

        /*
        @函数:根据图元在群组中的索引号更改指定文本群组对象的中单个文本内容
        @输入参数:
            strFileName		: 文档的绝对路径
            strTextName		: 群组图元对象名称
            iElementIndex	: 要更改内容的单个文本在文本群组对象图元列表中的索引号
            strTextNew		: 新的文本内容
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_ChangeTextInGroupByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strTextName, int iElementIndex, [MarshalAs(UnmanagedType.LPWStr)] string strTextNew);

        /*
        @函数:根据字体图元的名称更改文本的高度
        @输入参数:
            strFileName		: 文档的绝对路径
            strTextName		: 群组图元对象名称
            dHeight			: 要更改的高度
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SetTextHeightByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, double dHeight);

        /*
        @函数:根据字体图元的索引号更改文本的高度
        @输入参数:
            strFileName	: 文档的绝对路径
            iShapeIndex	: 群组图元索引号
            dHeight		: 要更改的高度
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SetTextHeightByIndex([MarshalAs(UnmanagedType.LPWStr)] string strFileName, Int32 iShapeIndex, double dHeight);

        /*
        @函数:获取二维码所有类型
        @输出参数:
            vDevID	  : 二维码类型
            iDevCount : 数量
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetTBarCodeType2([Out] STU_BARCODETYPE[] vDevID, ref int nTypeCount);

        /*
          @函数:根据字体图元的名称获取文本内容
          @输入参数:
              strFileName	: 文档的绝对路径
              strTextName	: 群组图元对象名称
          @输出参数:
              arrText		: 文本内容
          */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetTextByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strTextName, char[] arrText);

        /*
        @函数:根据字体图元的名称获取文本上次打标的内容
        @输入参数:
            strFileName	: 文档的绝对路径
            strEntity	: 群组图元对象名称
        @输出参数:
            arrLaserText		: 文本上次打标的内容
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetLastMarkTextByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntity, char[] arrLaserText);

        /*
        @函数:根据字体图元的名称获取指定文本对象的类型名称
        @输入参数:
            strFileName	: 文档的绝对路径
            strTextName	: 群组图元对象名称
        @输出参数:
            nFontType	: 不同的字体的类型，双线、单线字体、二维码、点阵
            arrFontName		: 类型名称
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_GetFontTypeNameByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strTextName, ref int nFontType, char[] arrFontName);

        /*
        @函数:根据字体图元的名称设置指定文本对象的类型名称
        @输入参数:
            strFileName	: 文档的绝对路径
            strTextName	: 群组图元对象名称
            nFontType	: 不同的字体的类型，双线、单线字体、二维码、点阵
            strText		: 类型名称
        */
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public delegate BslErrCode BSL_SetFontTypeNameByName([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strTextName,int nFontType, [MarshalAs(UnmanagedType.LPWStr)] string strFontName);


    //获取/设置图片参数(根据图元索引), Add by ysp on 2023-8-1
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate int BSL_PictureParamByIndex(
        int action,                         // 操作, 1 - 获取; 2 - 设置; 其他值无效
        [MarshalAs(UnmanagedType.LPWStr)] String szFileName,    // orzx文件
        int nIndex,                         // 图元索引
/*IntPtr*/ ref bool invert,         // 反转
/*IntPtr*/ ref bool gray,           // 灰度
/*IntPtr*/ ref double contrast,     // 对比度(-1到1)
/*IntPtr*/ ref double luminance,    // 亮度(-1到1)
/*IntPtr*/ ref bool dpiFixed,       // 固定DPI
/*IntPtr*/ int[] dpi,               // DPI的X和Y值, dpi[0]为X值, dpi[1]为Y值
/*IntPtr*/ ref bool net_dot,        // 网点
/*IntPtr*/ ref uint threshold,      // 阈值(0到255)
/*IntPtr*/ ref bool dual_direction, // 双向扫描
/*IntPtr*/ ref double dot_time,     // 打点时间(ms, 0到60)
/*IntPtr*/ ref bool adjust_power    // 调整功率
        );


    #endregion    文本图元相关属性

    #endregion    图元属性

    #region 扩展轴

    /*
    @函数:扩展轴移动到指定坐标位置
    @输入参数:
        szDevId :设备ID,
        axis	: 扩展轴轴号,0 = 轴0,1 = 轴1
        GoalPos	: 坐标绝对位置
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AxisMoveTo([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis, double GoalPos);

    /*
	@函数:扩展轴校正原点
	@输入参数:
		szDevId :设备ID,
		axis	: 扩展轴轴号,0 = 轴0,1 = 轴1
	*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AxisCorrectOrigin([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis);

    /*
	@函数:得到扩展轴的当前坐标
	@输入参数:
		szDevId :设备ID,
		axis	: 扩展轴轴号,0 = 轴0,1 = 轴1
	@返回值:坐标值
	*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate double BSL_GetAxisCoor([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis);

    /*
	@函数:扩展轴移动到指定脉冲坐标位置
	@输入参数:
		szDevId : 设备ID,
		axis	: 扩展轴轴号,0 = 轴0,1 = 轴1
		nGoalPos: 脉冲坐标位置
	*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_AxisMoveToPulse([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis, double GoalPos);


    /*
    @函数:获取扩展轴的当前脉冲坐标
    @输入参数:
    szDevId : 设备ID,
    axis	: 扩展轴轴号,0 = 轴0,1 = 轴1
    @返回值 : 当前脉冲坐标
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate int BSL_GetAxisCoorPulse([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis);

    /*
    @函数:复位扩展轴坐标
    @输入参数:
    szDevId : 设备ID,
    axis	: 扩展轴轴号,0 = 轴0,1 = 轴1
    bAll    : 所有的轴复位
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ResetAxisPos([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int axis, bool bAll);

    /*
		@函数:获取扩展轴参数
		@输入参数:
		    wchar_t* szParName,			//参数名
			int     axis,				//扩展轴  0 = 轴0  1 = 轴1
		@输出参数:
			ROTAXIS	：是否旋转轴 1-是，0-不是
			REVROT	：是否反转	1-反转，0-正转
			nAxisDir：用户设置的，扩展轴方向：0-3分别表示X,Y,Z,U
			nPPR	：每转脉冲数
			LPR		：每转螺间距,mm
			MINCOORD：最小坐标,mm
			MAXCOORD：最大坐标,mm
			MINPPS	：最小速度，脉冲/秒
			MAXPPS	：最大速度，脉冲/秒
			RUNPPS	：电机转速，脉冲/秒
			STTPPS	：电机启动速度, 脉冲/s ，STTPPS 必须小于 RUNPPS
			ACCTIME	：加速时间,ms
			nZeroType,			// 零点类型，0：低电平、1：高电平、2：脉冲
			fZeroPPS,			// 回0速度, 脉冲/s
			fZeroTimeout,		// 回0超时，s
			AccurToZero,		// 是否精确回零
			PPDEC,				// 减速比, 是旋转轴时有效
			nMoMode,			// 电机回零模式: 0-默认模式， 1-回零模式
			nMoOrignLevel,		// 电机电平: 1-原点是高电平，0-原点是低电平
			fOrgX,				//平台原点（零点）,用于分割与移动标刻，标刻过程中需使用
			fOrgY,				//平台原点（零点）,用于分割与移动标刻，标刻过程中需使用
			bAutoZero,			//是否自动回零，true x,y同时回零，否则都不回零,回零时按bBackOrigin指定的方式回
			bBackOrigin,		//运行方式是否直回原点,true:直回原点，false:原路返回,标刻过程中需使用
			//扩展轴校正参数
			DISLOCCOMPENSATE：错位补偿, mm,当机械制造的误差比较大时，在平面拼图加工时会导致错位的现象，调节此参数可消除错位现象。
			GAPCOMPENSATE：间隙补偿(mm),补偿在运动时齿轮间的间隙误差
			LIGHTOUTDEVIATION：出光时间误差(ms)
			MARKSPEEDDEVIATION：打标速度偏差(ms/s)
			ROTTIMEDEVIATION：旋转时间偏差(ms/s)
			POINTDISTENCE：两点间距离阀值(mm)
			GVSX：X桶形失真系数
			GVSY：Y桶形失真系数
			GVT：梯形失真系数
			Buffertime：缓冲延时
		*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetAxisParam(
        [MarshalAs(UnmanagedType.LPWStr)] string szDevId,           //设备ID
        [MarshalAs(UnmanagedType.LPWStr)] string szParName,         //参数名
        int axis,                       //扩展轴  0 = 轴0  1 = 轴1
        ref ushort ROTAXIS,           // 是否旋转轴 1-是，0-不是
        ref ushort REVROT,            // 是否反转	1-反转，0-正转
        ref int nAxisDir,          // 用户设置的，扩展轴方向：0-3分别表示X,Y,Z,U
        ref ushort nPPR,              // 每转脉冲数
        ref float LPR,               // 每转螺间距,mm
        ref float MINCOORD,          // 最小坐标,mm
        ref float MAXCOORD,          // 最大坐标,mm
        ref float MINPPS,            // 最小速度，脉冲/秒
        ref float MAXPPS,            // 最大速度，脉冲/秒
        ref float RUNPPS,            // 电机转速，脉冲/秒
        ref float STTPPS,            // 电机启动速度, 脉冲/s ，STTPPS 必须小于 RUNPPS
        ref float ACCTIME,           // 加速时间,ms

        ref ushort nZeroType,         // 零点类型，0：低电平、1：高电平、2：脉冲
        ref float fZeroPPS,          // 回0速度, 脉冲/s
        ref float fZeroTimeout,      // 回0超时，s
        ref ushort AccurToZero,       // 是否精确回零
        ref float PPDEC,             // 减速比, 是旋转轴时有效
        ref ushort nMoMode,           // 电机回零模式: 0-默认模式， 1-回零模式
        ref ushort nMoOrignLevel,     // 电机电平: 1-原点是高电平，0-原点是低电平

        ref float fOrgX,             //平台原点（零点）,用于分割与移动标刻，标刻过程中需使用
        ref float fOrgY,             //平台原点（零点）,用于分割与移动标刻，标刻过程中需使用
        ref bool bAutoZero,         //是否自动回零，true x,y同时回零，否则都不回零,回零时按bBackOrigin指定的方式回
        ref bool bBackOrigin,       //运行方式是否直回原点,true:直回原点，false:原路返回,标刻过程中需使用

        //扩展轴校正参数
        ref float DISLOCCOMPENSATE,  // 错位补偿, mm,当机械制造的误差比较大时，在平面拼图加工时会导致错位的现象，调节此参数可消除错位现象。
        ref float GAPCOMPENSATE,     // 间隙补偿(mm),补偿在运动时齿轮间的间隙误差

        ref float LIGHTOUTDEVIATION, // 出光时间误差(ms)
        ref float MARKSPEEDDEVIATION,// 打标速度偏差(ms/s)
        ref float ROTTIMEDEVIATION,  // 旋转时间偏差(ms/s)
        ref float POINTDISTENCE,     // 两点间距离阀值(mm)

        ref float GVSX,              //X桶形失真系数
        ref float GVSY,              //Y桶形失真系数
        ref float GVT,				    //梯形失真系数
        ref float fBuffertime		//缓冲延时
        );

    /*
    @函数:设置扩展轴参数
    @输入参数:
        szDevId				:设备ID
        szParName			:参数名
        axis				:扩展轴轴号，0 = 轴0，1 = 轴1
    @输出参数:
        ROTAXIS				:是否旋转轴 1-是，0-不是
        REVROT				:是否反转	1-反转，0-正转
        nPPR				:每转脉冲数
        LPR					:每转螺间距,mm
        MINCOORD			:最小坐标,mm
        MAXCOORD			:最大坐标,mm
        MINPPS				:最小速度，脉冲/秒
        MAXPPS				:最大速度，脉冲/秒
        RUNPPS				:电机转速，脉冲/秒
        STTPPS				:电机启动速度, 脉冲/s ，STTPPS 必须小于 RUNPPS
        ACCTIME				:加速时间,ms
        nZeroType			:零点类型，0：低电平、1：高电平、2：脉冲
        fZeroPPS			:回0速度, 脉冲/s
        fZeroTimeout		:回0超时，s
        AccurToZero			:是否精确回零
        PPDEC				:减速比, 是旋转轴时有效
        nMoMode				:电机回零模式: 0-默认模式， 1-回零模式
        nMoOrignLevel		:电机电平: 1-原点是高电平，0-原点是低电平
        DISLOCCOMPENSATE	:错位补偿, mm,当机械制造的误差比较大时，在平面拼图加工时会导致错位的现象，调节此参数可消除错位现象。
        GAPCOMPENSATE		:间隙补偿(mm),补偿在运动时齿轮间的间隙误差
        fBuffertime			:缓冲延时
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SetAxisParam(
       [MarshalAs(UnmanagedType.LPWStr)] string szDevId,           //设备ID
       [MarshalAs(UnmanagedType.LPWStr)] string szParName,         //参数名
       int axis,                       //扩展轴  0 = 轴0  1 = 轴1
       ushort ROTAXIS,           // 是否旋转轴 1-是，0-不是
       ushort REVROT,            // 是否反转	1-反转，0-正转
       ushort nPPR,              // 每转脉冲数
       float LPR,               // 每转螺间距,mm
       float MINCOORD,          // 最小坐标,mm
       float MAXCOORD,          // 最大坐标,mm
       float MINPPS,            // 最小速度，脉冲/秒
       float MAXPPS,            // 最大速度，脉冲/秒
       float RUNPPS,            // 电机转速，脉冲/秒
       float STTPPS,            // 电机启动速度, 脉冲/s ，STTPPS 必须小于 RUNPPS
       float ACCTIME,           // 加速时间,ms
       ushort nZeroType,         // 零点类型，0：低电平、1：高电平、2：脉冲
       float fZeroPPS,          // 回0速度, 脉冲/s
       float fZeroTimeout,      // 回0超时，s
       ushort AccurToZero,       // 是否精确回零
       float PPDEC,             // 减速比, 是旋转轴时有效
       ushort nMoMode,           // 电机回零模式: 0-默认模式， 1-回零模式
       ushort nMoOrignLevel,     // 电机电平: 1-原点是高电平，0-原点是低电平

       //扩展轴校正参数
       float DISLOCCOMPENSATE,  // 错位补偿, mm,当机械制造的误差比较大时，在平面拼图加工时会导致错位的现象，调节此参数可消除错位现象。
       float GAPCOMPENSATE,     // 间隙补偿(mm),补偿在运动时齿轮间的间隙误差
       float fBuffertime		//缓冲延时
       );

    //显示扩展轴参数对话框
    //输入参数:  无
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_ShowAxisCfgDlg([MarshalAs(UnmanagedType.LPWStr)] string szParName);


    #endregion

#region 端口
    /*
	@函数:获取硬件输入端口状态，支持8个输入口
	@输入参数:
		szDevId	: 设备ID
    @输出参数:
		data	: 状态值，8位二进制（0-255）
	*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ReadPort([MarshalAs(UnmanagedType.LPWStr)] string strDevID, ref UInt16 data);

    /*
      @函数:获取硬件输出端口状态，支持8个输出口
      @输入参数:
          szDevId	: 设备ID
      @输出参数:
          data	: 状态值，8位二进制（0-255）
      */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ReadOutPort([MarshalAs(UnmanagedType.LPWStr)] string strDevID, ref UInt16 data);
    /*
    @函数:设置硬件输出端口状态，支持8个输出口
    @输入参数:
        szDevId	: 设备ID
        portNum :输出端口, 目前有效端口为0-7
        nMode	:输出模式, 0-电平，1-脉冲，2-跳变
        nLevel	:输出电平, 0-低电平，1-高电平
        nPulse  :脉冲周期 单位为ms 当 nMode=1脉冲模式时有效，上位机模拟延时，例：nPulse = 300ms,则接口等待300ms才会返回
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_WritePort([MarshalAs(UnmanagedType.LPWStr)] string strDevID, UInt16 portNum, UInt16 nMode, UInt16 nLevel, UInt16 nPulse);

    /*
	@函数:获取激光器四个输入口状态，支持16、21、11、12个输出口
	@输入参数:
		szDevId	: 设备ID
	@输出参数:
		data	: 状态值，4位二进制（0-16）
	*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetLaserPortState([MarshalAs(UnmanagedType.LPWStr)] string strDevID, ref UInt16 data);
    #endregion

    #region  振镜校正

    //手工校正
    //**************************************************************************************
    //修改当前数据库中的手工校正参数
    //输入参数：szParName  激光参数名称
    //输入参数: dScaleX  X轴方向的放缩比例
    //输入参数: dScaleY  Y轴方向的放缩比例
    //输入参数: dDistorX  X轴方向的桶形失真系数
    //输入参数: dDistorY  Y轴方向的桶形失真系数
    //输入参数: dHorverX  X轴方向的平行四边形失真系数
    //输入参数: dHorverY  Y轴方向的平行四边形失真系数
    //输入参数: dTrapedistorX  X轴方向的梯形失真系数
    //输入参数: dTrapedistorY  Y轴方向的梯形失真系数
    //输入参数: bSaveToFile  是否保存到标准配置文件(BslCAD.cfg)
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ModifyManualCorPara([MarshalAs(UnmanagedType.LPWStr)] string szParName,
    double dScaleX,
    double dScaleY,
    double dDistorX,
    double dDistorY,
    double dHorverX,
    double dHorverY,
    double dTrapedistorX,
    double dTrapedistorY,
    bool bSaveToFile);

    /*///////////////////////////////
    _________________  
	|		 |		  | X1
	|		 |		  |
	|		 | a 	  |	
	|--------|--------| X2
	|        |		  |
	|		 |		  |
	|________|________| X3
	Y1       Y2      Y3
    /*///////////////////////////////
       //根据当前数据库中手工校正参数和实际测量数据计算产生推荐值
       //输入参数: dx_T  X轴方向理论边长 mm
       //输入参数: dy_T  Y轴方向理论边长 mm
       //输入参数: da_A  角度a的实际测量角度(°)
       //输入参数: dx1_A  实际测量的X1的长度 mm
       //输入参数: dx2_A  实际测量的X2的长度 mm
       //输入参数: dx3_A  实际测量的X3的长度 mm
       //输入参数: dy1_A  实际测量的Y1的长度 mm
       //输入参数: dy2_A  实际测量的Y2的长度 mm
       //输入参数: dy3_A  实际测量的Y3的长度 mm
       //输出参数: dScaleX  X轴方向的放缩比例
       //输出参数: dScaleY  Y轴方向的放缩比例
       //输出参数: dDistorX  X轴方向的桶形失真系数
       //输出参数: dDistorY  Y轴方向的桶形失真系数
       //输出参数: dHorverX  X轴方向的平行四边形失真系数
       //输出参数: dHorverY  Y轴方向的平行四边形失真系数
       //输出参数: dTrapedistorX  X轴方向的梯形失真系数
       //输出参数: dTrapedistorY  Y轴方向的梯形失真系数
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GenerateRecommendedValues(
    double dx_T,
    double dy_T,
    double da_A,
    double dx1_A,
    double dx2_A,
    double dx3_A,
    double dy1_A,
    double dy2_A,
    double dy3_A,
    ref double dScaleX,
    ref double dScaleY,
    ref double dDistorX,
    ref double dDistorY,
    ref double dHorverX,
    ref double dHorverY,
    ref double dTrapedistorX,
    ref double dTrapedistorY
    );

    //获取当前数据库中的手工校正参数
    //输入参数：szParName  将要设置的参数名称
    //输出参数: dScaleX  X轴方向的放缩比例
    //输出参数: dScaleY  Y轴方向的放缩比例
    //输出参数: dDistorX  X轴方向的桶形失真系数
    //输出参数: dDistorY  Y轴方向的桶形失真系数
    //输出参数: dHorverX  X轴方向的平行四边形失真系数
    //输出参数: dHorverY  Y轴方向的平行四边形失真系数
    //输出参数: dTrapedistorX  X轴方向的梯形失真系数
    //输出参数: dTrapedistorY  Y轴方向的梯形失真系数
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetManualCorPara([MarshalAs(UnmanagedType.LPWStr)] string szParName,
        ref double dScaleX,
        ref double dScaleY,
        ref double dDistorX,
        ref double dDistorY,
        ref double dHorverX,
        ref double dHorverY,
        ref double dTrapedistorX,
        ref double dTrapedistorY
    );
    //**************************************************************************************

    //多点校正（九点校正）
    //**************************************************************************************

    //显示九点校正 
    //输入参数:iType=1九点校正
    //输入参数: strParName  激光参数名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_ShowCaliDlg(int iType, [MarshalAs(UnmanagedType.LPWStr)] string strParName);

    //校正文件初始化参数
    //输入参数: 
    // szFileName -校正文件路径
    // szParName - 激光参数名称
    // fWorkSize - 振镜大小
    // fValSize  - 检定区域
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_InitCaliFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strParName, float fWorkSize, double fValSize);

    //设置XY反向标志
    //输入参数: iXYFlip:是否xy互换 iXInvert:是否x反向 iYInvert:是否y反向
    //输入参数: strParName  激光参数名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_SetXYFlipFlag(int iXYFlip, int iXInvert, int iYInvert, [MarshalAs(UnmanagedType.LPWStr)] string strParName);

    //设置场镜大小
    //输入参数: fWorkSize   场镜大小
    //输入参数: strParName  激光参数名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_SetWorkSize(float fWorkSize, [MarshalAs(UnmanagedType.LPWStr)] string strParName);

    //设置是否使用校正文件
    //输入参数: bEnable - 是否使能校正文件
    //输入参数: strParName  激光参数名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_SetCalFileFlag(bool bEnable, [MarshalAs(UnmanagedType.LPWStr)] string strParName);

    //标刻九点矩形
    //输入参数: szDevId			设备ID
    //输入参数: strCalFileName  校正文件路径
    //输入参数: fValSize		有效检定区域 mm
    //输入参数: fTagSize		校正标记尺寸 mm
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkNinePointRect([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string strCalFileName, double fValSize, double fTagSize);

    //标刻最大图形
    //输入参数: szDevId			设备ID
    //输入参数: strCalFileName  校正文件路径
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkMaxShape([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string strCalFileName);

    //多点校正标刻
    //输入参数: szDevId			设备ID
    //输入参数: strCalFileName  校正文件路径
    //输入参数: fValSize		有效检定区域 mm
    //输入参数: fTagSize		校正标记尺寸 mm
    //输入参数: nIndex			校正点数列表当前索引    0: 3*3   1: 5*5   2: 9*9   3: 17*17   4: 33*33   5: 65*65
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkMultiCalPoint([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string strCalFileName, double fValSize, double fTagSize, int nIndex);

    //导入测量值
    //输入参数: nIndex			校正点数列表当前索引    0: 3*3   1: 5*5   2: 9*9   3: 17*17   4: 33*33   5: 65*65
    //输出参数: vCalData		网格检定点实测数据
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_LoadMeasuredValueFile2(int nIndex, [Out] gridCalData[] pCalData, ref int nDataCount);

    //标刻验证图形
    //输入参数: szDevId			设备ID
    //输入参数: strCalFileName  校正文件路径
    //输入参数: nIndex			校正点数列表当前索引    0: 3*3   1: 5*5   2: 9*9   3: 17*17   4: 33*33   5: 65*65
    //输入参数: nGap			线框间隔 mm  min(nGap, 5.0)  一般取5   
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkCheckShape([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string strCalFileName, int nIndex, double nGap);

    //设置校正文件参数名
    //输入参数: strFileName  校正文件名
    //输入参数: strParName  激光参数名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_SetCalFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strParName);

    //获取校正文件参数名
    //输入参数: strFileName  校正文件名
    //输出参数: strParName  激光参数名称
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_GetCalFile([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strParName);

    //加载九点校正文件
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_LoadNewCalFile2([Out]STU_FILENAME[] vFileName, ref int iNameCount);

    //保存校正文件
    //输入参数: strCalFileName  校正文件路径
    //输入参数: nIndex			校正点数列表当前索引    0: 3*3   1: 5*5   2: 9*9   3: 17*17   4: 33*33   5: 65*65
    //输入参数: vCalData		网格检定点理论和实测数据
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SaveCalFile2([MarshalAs(UnmanagedType.LPWStr)] string strCalFileName, [MarshalAs(UnmanagedType.LPWStr)] string szParName, int nIndex, gridCalData[] pCalData, int nDataCount);

    //获取校正参数
    //输入参数: strCalFileName  校正文件路径
    //输入参数: nIndex			校正点数列表当前索引    0: 3*3   1: 5*5   2: 9*9   3: 17*17   4: 33*33   5: 65*65 
    //输出参数: pCalData		网格检定点理论和实测数据
    //输出参数:	nDataCount:		数据的数量
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetCalData([MarshalAs(UnmanagedType.LPWStr)] string strCalFileName, int nIndex, [Out] gridCalData[] pCalData, ref int nDataCount);

    //第三方的校正接口回调函数定义
    //输出参数 szDevID:设备ID
    //输入参数 fPx:图纸坐标x
    //输入参数 fPy:图纸坐标y
    //输出参数 fSx:振镜坐标x
    //输出参数 fSy:振镜坐标y
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool BSL_InvokeSetMMtoScannerPosCallBack([MarshalAs(UnmanagedType.LPWStr)] string szDevID, float fPx, float fPy, ref float fSx, ref float fSy);

    //使用第三方的校正接口
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_SetMMtoScannerPosCallBack(BSL_InvokeSetMMtoScannerPosCallBack func);
    //**************************************************************************************   
    #endregion

    #region 视觉专用
    //移动振镜
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GoToPos([MarshalAs(UnmanagedType.LPWStr)] string strDevID, double x1, double y1);

    /*
    @函数:设置单卡的旋转角度(度)，偏移(mm)，旋转中心(mm)，对所有图形 
    @输入参数:
        szDevId	: 设备ID
        dx: x方向偏移值
        dy: y方向偏移值
        cx: 旋转中心x的坐标
        cy:	旋转中心y的坐标
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_SetOffsetValues(
    double dAngle,      //旋转角度(度)
    double dOffsetX,    //动态偏移x(mm)
    double dOffsetY,    //动态偏移y(mm)
    double dCenterX,    //旋转中心x(mm)
    double dCenterY);   //旋转中心y(mm)

    /*
	@函数:设置设备关联的参数 旋转角度（度），偏移（mm），旋转中心（mm）
	@输入参数:
		szDevId	: 设备ID
		szFileName 模板文件
		szParName  设置的参数名称
		r   旋转角度
		dx  X偏移
		dy  Y偏移
		cx  旋转中心X坐标
		cy  旋转中心Y坐标
	*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_SetOffsetValuesOfDevice(
    [MarshalAs(UnmanagedType.LPWStr)] string szDevId,
    [MarshalAs(UnmanagedType.LPWStr)] string szFileName,
    [MarshalAs(UnmanagedType.LPWStr)] string szParName,
    double dAngle,
    double dOffsetX,
    double dOffsetY,
    double dCenterX,
    double dCenterY);

    /*
    @函数:多组偏移旋转标刻
	@输入参数： 
        szDevId :设备ID
	    szDocName :图形文件名称
	    offsetArr :偏移旋转数组
	    nArrSize  :数组大小
	    nShapeIndex :图形索引  nShapeIndex=-1,表示标刻图形文件中所有图形
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkMultiShapesByOffsetAndRot([MarshalAs(UnmanagedType.LPWStr)] string strDevID, [MarshalAs(UnmanagedType.LPWStr)] string strDocName, Offset[] offsetArr, int nArrSize, int nShapeIndex);


    /**
      * 获取文件中所有打标路径数据
      * szDevId：设备ID
      * szDocName: 文件名
      * bRotOffset: 是否做偏移旋转
      * r   旋转角度
      * dx  X偏移
      * dy  Y偏移
      * cx  旋转中心X坐标
      * cy  旋转中心Y坐标
      * nCount 输出数目
      * pMarkPaths：输出标刻数据
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetMarkDataPaths2([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string szDocName,
         bool bRotOffset, double r, double dx, double dy, double cx, double cy,
         ref int nCount, IntPtr pMarkPaths);

    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetMarkDataPaths3([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string szDocName,
         bool bRotOffset, double r, double dx, double dy, double cx, double cy,
         ref int nCount, [Out] PathDataShape[] MarkPaths);

    //标刻路径
    /**
      * 标刻路径点
      * szDevId：设备ID
      * nCount 数目
      * pMarkPaths：标刻数据
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkDocDataPaths2([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int nCount, IntPtr pMarkPaths, [MarshalAs(UnmanagedType.LPWStr)] string szParName);

    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkDocDataPaths3([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int nCount, PathDataShape[] pMarkPaths, [MarshalAs(UnmanagedType.LPWStr)] string szParName);


    /**获取模板文件图元标刻轮廓和填充路径
  * 输入参数： szDocName --图形文件名称
  * 输入参数： iShapeIndex --对象在图形列表中的索引号  iShapeIndex = -1 获取文件所有图元轮廓和填充路径
  * 输出参数： nOutCount 轮廓数
  * 输出参数： pOutPaths --轮廓路径
  * 输出参数： nFillCount 填充数
  * 输出参数： pFillPaths --填充路径
  */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate int BSL_GetShapeMarkOutPathsCount([MarshalAs(UnmanagedType.LPWStr)] string szDocName, int iShapeIndex);

    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate int BSL_GetShapeMarkFillPathsCount([MarshalAs(UnmanagedType.LPWStr)] string szDocName, int iShapeIndex);

    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetShapeMarkPaths2([MarshalAs(UnmanagedType.LPWStr)] string szDocName, int iShapeIndex,
    ref int nOutCount, [Out] PathDataShape[] pOutPaths, ref int nFillCount, [Out] PathDataShape[] pFillPaths);

    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetShapeMarkPaths2NoJump([MarshalAs(UnmanagedType.LPWStr)] string szDocName, int iShapeIndex,
    ref int nOutCount, [Out] PathDataShape[] pOutPaths, ref int nFillCount, [Out] PathDataShape[] pFillPaths);

    /** 路径标刻
      * 输入参数： szDevId --设备ID
      * 输入参数： nCount --路径数
      * 输入参数： pMarkPaths --标刻数据
      * 输入参数： szParName --配置参数名称
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkShapePaths2([MarshalAs(UnmanagedType.LPWStr)] string szDevId, int nCount, PathDataShape[] pMarkPaths, int nPenNo, [MarshalAs(UnmanagedType.LPWStr)] string szParName);

    #endregion 视觉专用

#region 标刻
    /*
    @函数:设置硬件触发端口
    @输入参数:
        iPortNum	: 端口号
        szParName	: 参数名称
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_SetHardwareIOPort(int iPortNum, [MarshalAs(UnmanagedType.LPWStr)] string szParName);

    /*
    @函数:获取硬件触发端口
    @输入参数:
        iPortNum	: 端口号
        szParName	: 参数名称
     */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void BSL_GetHardwareIOPort(ref int iPortNum, [MarshalAs(UnmanagedType.LPWStr)] string szParName);

    /*
	@函数:硬件触发标刻选定的设备的关联数据文件
    @输入参数:
		szDevId	     :设备ID
        nMarkCount   :标刻总数
        bContinue    :是否连续标刻
        nTriggerPort :硬件触发端口号
        nTriggerLevel:硬件触发电平
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkByDeviceIdOfHardwareTrigger([MarshalAs(UnmanagedType.LPWStr)] string strDevID, int nMarkCount, bool bContinue, int nTriggerPort, int nTriggerLevel);


    /*
      @函数:软件控制触发硬件标刻
      @输入参数:
          szDevId	     :设备ID
     */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SoftTiggerHardwareMark([MarshalAs(UnmanagedType.LPWStr)] string strDevID);

    /*
	@函数:获取是否已经进入硬件触发状态
	@输入参数:
		szDevId	     :设备ID
	*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_GetIsWaitTriggerByDevId([MarshalAs(UnmanagedType.LPWStr)] string strDevID);

    /*
      @函数:是否已经停止打标
      @输入参数:
         szDevId:设备ID
     */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_IsStopMark([MarshalAs(UnmanagedType.LPWStr)] string strDevID);

    /*
    @函数:标刻选定的设备的关联数据文件
    @输入参数:
        szDevId	 :设备ID
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkByDeviceId([MarshalAs(UnmanagedType.LPWStr)] string strDevID);

    /*
     @函数:标刻选定的设备的关联数据文件
     @输入参数:
         szDevId	     :设备ID
         nMarkCount      :标刻次数
         bContinue       :是否连续打标
     */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkByDeviceIdEx([MarshalAs(UnmanagedType.LPWStr)] string strDevID, int nMarkCount, bool bContinue);

    /*
     @函数:标刻选定的设备的与选定的文档名称
     @输入参数:
         szDevId	     :设备ID
         strFileName     :文档名称
         nMarkCount      :标刻次数
         bContinue       :是否连续打标
     */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate int BSL_MarkSelectedFile([MarshalAs(UnmanagedType.LPWStr)] string strDevID, [MarshalAs(UnmanagedType.LPWStr)] string strFileName, int nMarkCount, bool bCountinue);

    /*
     @函数:紧急停止
     @输入参数:
         szDevId:设备ID
     */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_EmergenyStop([MarshalAs(UnmanagedType.LPWStr)] string szDevId);

    /*
      @函数:停止打标
      @输入参数:
          szDevId:设备ID
     */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_StopMark([MarshalAs(UnmanagedType.LPWStr)] string strDevID);

    /*
      @函数:标刻当前文件内的指定对象
      @输入参数:
          szDevId	   :设备ID
          szFileName :文档路径
          strEntName :要加工的指定对象的名称
      */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkEntity([MarshalAs(UnmanagedType.LPWStr)] string strDevID, [MarshalAs(UnmanagedType.LPWStr)] string szFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName);

    /*
    @函数:屏蔽一块区域不标刻
    @输入参数:
        strFileName	:文档路径
        vPoints		:区域的点节点
        iPtCount	:点的数量
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MaskArea([MarshalAs(UnmanagedType.LPWStr)] string strFileName, POINTF[] vPoints, int iPtCount);

    //选定设备显示红光
    //szDevId-设备ID
    //bCountinue - 是否连续显示红光
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_RedLightMark([MarshalAs(UnmanagedType.LPWStr)] string strDevID, bool bCountinue);

    //选定设备与选定图元显示红光
    //szDevId-设备ID
    //输入参数: strEntName 要加工的指定对象的名称
    //int[] vShpIndex 要红光的图元序号
    //int iShpCount 要红光的图元个数
    //bCountinue - 是否连续显示红光
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_RedLightMarkByEnt2([MarshalAs(UnmanagedType.LPWStr)] string strDevID, [MarshalAs(UnmanagedType.LPWStr)] string szFileName, int[] vShpIndex, int iShpCount, bool bCountinue);

    //在一点持续出光一段时间
    //keep-出光时间， ms
    //pen - 使用的笔号
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_LarseOnPoint([MarshalAs(UnmanagedType.LPWStr)] string strDevID, double x, double y, double keep, int pen, bool bRedlight);

    //标刻点
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkPoint([MarshalAs(UnmanagedType.LPWStr)] string strDevID, double x, double y, double delay, int pen);

    //标刻一组点
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkPoints([MarshalAs(UnmanagedType.LPWStr)] string strDevID, POINTF[] vPoints, int iPtCount, int nPenNum);

    //标刻一组线段 统一笔号
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkLines([MarshalAs(UnmanagedType.LPWStr)] string strDevID, POINTF[] lines, int iLineCount, int[] iPtCount, int penNum);

    //标刻一组线段 笔号penNum是个数组
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkLinesX([MarshalAs(UnmanagedType.LPWStr)] string strDevID, POINTF[] lines, int iLineCount, int[] iPtCount, int[] penNum);

    //标刻一个分组内的图元
    //输入参数：szDevId 设备ID
    //          group 图元骨架信息容器
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkEntitiesInGroup2([MarshalAs(UnmanagedType.LPWStr)] string szDevId, AreaEntityGroupCSharp groupC);
 #endregion 标刻

#region 扩展功能
    //导出G-CODE
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ExportGCode2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [Out] STU_GCODE[] wstrGCode, ref int nCount);

    //导出G-CODE  by name
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ExportGCodeByName2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, [MarshalAs(UnmanagedType.LPWStr)] string strEntName, [Out] STU_GCODE[] wstrGCode, ref int nCount);

    //导出G-CODE by index
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ExportGCodeByIndex2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, int iIndex, [Out] STU_GCODE[] wstrGCode, ref int nCount);

    //导出G-CODE by group
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_ExportGCodeByGroup2([MarshalAs(UnmanagedType.LPWStr)] string strFileName, AreaEntityGroupCSharp groupC, [Out] STU_GCODE[] wstrGCode, ref int nCount);

    //向指定的IP和端口发送字符串
    //输入参数: m_dwIpAddress	IP地址
    //			m_strPort       端口号
    //          m_dwConnectTimeout     连接超时
    //          m_dwReceiveTimeout     接收超时
    //          sendStr                发送内容
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_SendStrByTcpIp(UInt32 m_dwIpAddress, [MarshalAs(UnmanagedType.LPWStr)] string m_strPort, UInt32 m_dwConnectTimeout, UInt32 m_dwReceiveTimeout, [MarshalAs(UnmanagedType.LPWStr)] string sendStr);

    /*
     * 根据传入的分块中心位置找到对应的块并打标
     * szDocName: 文件名
     * type ：分块类型 0为根据长宽分块
     * x：x反向分块长或者X方向分块数
     * y：y方向分块宽或者Y方向分块数
     * centralPoint：分块中心点位置
    */
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_FindBlockMark([MarshalAs(UnmanagedType.LPWStr)] string szDocName, int type, double x, double y, POINTF centralPoint);

    //分块标刻图形文件中的图形对象
    //标刻的范围按所有图形的最大外接矩形大小，标刻时会自动将分块中心与振镜中心对齐
    //输入参数: szDevId 设备ID
    //          szDocName 要加工的文件名称
    //			dx-分块的宽度
    //			dy-分块的高度
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate BslErrCode BSL_MarkBlockByDoc([MarshalAs(UnmanagedType.LPWStr)] string szDevId, [MarshalAs(UnmanagedType.LPWStr)] string szDocName, double dx, double dy);

    //设置DA(0-10V)
    //szDevId:设备ID
    //nLaserPower:功率百分比值（0-1）
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool BSL_SetDAValue([MarshalAs(UnmanagedType.LPWStr)] string szDevId,float nLaserPower);

    #endregion 扩展功能

    public class DllInvoke
    {
        #region Win API
        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(string path);

        [DllImport("kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr lib, string funcName);

        [DllImport("kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr lib);
        #endregion

        public IntPtr hLib;
        public DllInvoke(String DLLPath)
        {
            hLib = LoadLibrary(DLLPath);
        }

        ~DllInvoke()
        {
            //不能显示释放dll，会导致dll中的线程无法正常结束,dll中的ExitInstance()函数会自动完成资源的释放
            // if (hLib != IntPtr.Zero)
            //  FreeLibrary(hLib);
        }

        //将要执行的函数转换为委托
        public Delegate GetFunctionAddress(string APIName, Type t)
        {
            IntPtr api = GetProcAddress(hLib, APIName);
            if (api == IntPtr.Zero)
            {
                Console.WriteLine("加载api: " + APIName + "失败");
                return null;
            }

            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }
    }//    public class DllInvoke
};
