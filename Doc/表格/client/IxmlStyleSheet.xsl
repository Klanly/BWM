<?xml version="1.0" encoding="gb2312"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/config">
		<html>
			<head>
				<title>
					表格<xsl:value-of select="./readme/configname"/>的中间数据
				</title>
			</head>
			<body>
				<ul>
					<li>
						源配置文件名：<xsl:value-of select="./readme/configname"/>
					</li>
					<li>
						文件版本：<xsl:value-of select="./readme/version"/>
					</li>
				</ul>
				<hr></hr>
				<!--输出表格定义-->
				<xsl:for-each select="./tabledata">
					<br>
						<strong>
							<a>
								<xsl:attribute name="name">define<xsl:value-of select="@name"/></xsl:attribute>
								<xsl:value-of select="@name"/>表格定义
							</a>
						</strong>
						<a>
							<xsl:attribute name="href">
								#<xsl:value-of select="@name"/>
							</xsl:attribute>
							<small>转到数据</small>
						</a>
					</br>
					<br>
						<code>
							(Excel文件：<xsl:value-of select="./info/table/@ExcelFile"/>　　表单：<xsl:value-of select="./info/table/@sheet"/>　　排序：<xsl:value-of select="./info/table/@sort"/>)
						</code>
					</br>
					<table border ="1" width="100%">
						<tr>
							<th align="left" bgcolor="Teal">Excel字段名</th>
							<th align="left" bgcolor="Teal">类型</th>
							<th align="left" bgcolor="Teal">大小(字节)</th>
							<th align="left" bgcolor="Teal">程序字段名</th>
						</tr>
						<xsl:for-each select="./info/table/fields/field">
							<tr>
								<td>
									<xsl:value-of select="@name"/>
								</td>
								<td>
									<xsl:value-of select="@type"/>
								</td>
								<td>
									<xsl:value-of select="@size"/>
								</td>
								<td>
									<xsl:value-of select="@var"/>
								</td>
							</tr>
						</xsl:for-each>
					</table>
				</xsl:for-each>
				<br></br>
				<hr>
				</hr>
				<!--输出表格数据-->
				<xsl:for-each select="./tabledata">
					<br>
						<strong>
							<a>
								<xsl:attribute name="name">
									<xsl:value-of select="@name"/>
								</xsl:attribute>
								<xsl:value-of select="@name"/>
							</a>
						</strong>
						<xsl:text> </xsl:text>
						<a>
							<xsl:attribute name="href">#define<xsl:value-of select="@name"/></xsl:attribute>
							<small>转到定义</small>
						</a>
					</br>
					<table border="1" width="100%">
						<tr>
							<xsl:for-each select="./info/table/fields/field">
								<th align="left" bgcolor="Silver">
									<big>
										<xsl:value-of select="@name"/>
									</big>
								</th>
							</xsl:for-each>
						</tr>
						<xsl:for-each select="./data">
							<tr>
								<xsl:for-each select="./item">
									<td>
										<xsl:value-of select="."/>
									</td>
								</xsl:for-each>
							</tr>
						</xsl:for-each>
					</table>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>