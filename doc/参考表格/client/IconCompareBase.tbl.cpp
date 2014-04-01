// 自动生成代码
#include "stdafx.h"
#include "IconCompareBase.tbl.h"

namespace table
{
	CTableIconCompareBase CTableIconCompareBase::s_instance;
#ifdef _DEBUG
	std::string IconCompareBase::ToString() const
	{
		std::ostringstream oss;
		oss << key << ", "
			<< dwType << ", "
			<< dwID << ", "
			<< strname << ", "
			<< dwIcon << ", "
			<< dwBigIconGrp << ", "
			<< dwSmallIconGrp << ", "
			<< strParticle;
		return oss.str();
	}
#endif

	TABLE_IMPLEMENT_FUNC(IconCompareBase);
}
