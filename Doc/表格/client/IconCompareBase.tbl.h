// �Զ����ɴ���
#pragma once

namespace table
{
	//------------------------------------
	// �����ļ���ͼ����ձ�.xlsm
	// ������ƣ�IconCompareBase
	// ��¼��С��28�ֽ�
	// �ֶ�������7
	// ��������ID+����
	//------------------------------------
#pragma pack(1)
	struct IconCompareBase
	{
		union
		{
			QWORD       key;                                    // ����ؼ���
			struct
			{
				DWORD       dwType;                                 // ����
				DWORD       dwID;                                   // ID
			};
		};
		const char* strname;                                // ����
		DWORD       dwIcon;                                 // ͼ��
		DWORD       dwBigIconGrp;                           // ��ͼ����
		DWORD       dwSmallIconGrp;                         // Сͼ����
		const char* strParticle;                            // ������Ч

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

