// �Զ����ɴ���
#pragma once
//------------------------------------
// �����ļ������ٿ�쭵��߱�.xlsm
// ��    ��
//------------------------------------
#pragma pack(1)
struct RunningItemDataBase
{
	DWORD getUniqueID() const { return usage_id_hash(); }

	DWORD       dwField0;                               ///< ����ID
	char        strField1[32];                          ///< ��������
	DWORD       dwField2;                               ///< �ͷŶ���
	DWORD       dwField3;                               ///< ��ø���
};
#pragma pack()

#if false
namespace table
{
	/**
	 * \brief ���ٿ�쭵��߱�
	 */
	struct zRunningItemDataBaseEntry : public zEntry
	{
		DWORD       dwField0;                               ///< ����ID
		char        strField1[32];                          ///< ��������
		DWORD       dwField2;                               ///< �ͷŶ���
		DWORD       dwField3;                               ///< ��ø���

		const char* getClassName() const { return "���ٿ�쭵��߱�"; }
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

