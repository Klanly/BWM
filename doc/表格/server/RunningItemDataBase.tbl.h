// 自动生成代码
#pragma once
//------------------------------------
// 数据文件：极速狂飙道具表.xlsm
// 排    序：
//------------------------------------
#pragma pack(1)
struct RunningItemDataBase
{
	DWORD getUniqueID() const { return usage_id_hash(); }

	DWORD       dwField0;                               ///< 道具ID
	char        strField1[32];                          ///< 道具名字
	DWORD       dwField2;                               ///< 释放对象
	DWORD       dwField3;                               ///< 获得概率
};
#pragma pack()

#if false
namespace table
{
	/**
	 * \brief 极速狂飙道具表
	 */
	struct zRunningItemDataBaseEntry : public zEntry
	{
		DWORD       dwField0;                               ///< 道具ID
		char        strField1[32];                          ///< 道具名字
		DWORD       dwField2;                               ///< 释放对象
		DWORD       dwField3;                               ///< 获得概率

		const char* getClassName() const { return "极速狂飙道具表"; }
		void fill(const RunningItemDataBase& base)
		{
			this->dwField0 = base.dwField0;
			strncpy(this->strField1, base.strField1, 32);
			this->dwField2 = base.dwField2;
			this->dwField3 = base.dwField3;
		}
		void reset()
		{
			this->dwField0 = 0;
			this->strField1[0] = '\0';
			this->dwField2 = 0;
			this->dwField3 = 0;
		}
	};
	typedef zDataBM<zRunningItemDataBaseEntry, RunningItemDataBase> RunningItemDataBaseManager;
}
#endif

