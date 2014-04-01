// 自动生成代码
#pragma once

namespace table
{
	//------------------------------------
	// 数据文件：图标对照表.xlsm
	// 表格名称：IconCompareBase
	// 记录大小：28字节
	// 字段数量：7
	// 联合排序：ID+类型
	//------------------------------------
#pragma pack(1)
	struct IconCompareBase
	{
		union
		{
			QWORD       key;                                    // 排序关键字
			struct
			{
				DWORD       dwType;                                 // 类型
				DWORD       dwID;                                   // ID
			};
		};
		const char* strname;                                // 名称
		DWORD       dwIcon;                                 // 图标
		DWORD       dwBigIconGrp;                           // 大图标组
		DWORD       dwSmallIconGrp;                         // 小图标组
		const char* strParticle;                            // 粒子特效

		TABLE_RECORD_FUNC();
	};
#pragma pack()

	class CTableIconCompareBase : public CTableBase<IconCompareBase>
	{
	public:
		IconCompareBase* BinarySearch(DWORD dwID, DWORD dwType)
		{return table::BinarySearch<IconCompareBase, QWORD>(s_instance, (((QWORD)dwID) << 32) | (QWORD)dwType);}
	private:
		static CTableIconCompareBase s_instance;
	public:
		TABLE_TABLEMANAGER_FUNC(IconCompareBase);
	};

	// CTableIconCompareBase& GetTableIconCompareBase();
	TABLE_GET_TABLE_MANAGER(IconCompareBase);
}

