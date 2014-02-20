<?xml version="1.0" encoding="gb2312"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/config">
		<html>
			<head>
				<title>
					���<xsl:value-of select="./readme/configname"/>���м�����
				</title>
			</head>
			<body>
				<ul>
					<li>
						Դ�����ļ�����<xsl:value-of select="./readme/configname"/>
					</li>
					<li>
						�ļ��汾��<xsl:value-of select="./readme/version"/>
					</li>
				</ul>
				<hr></hr>
				<!--��������-->
				<xsl:for-each select="./tabledata">
					<br>
						<strong>
							<a>
								<xsl:attribute name="name">define<xsl:value-of select="@name"/></xsl:attribute>
								<xsl:value-of select="@name"/>�����
							</a>
						</strong>
						<a>
							<xsl:attribute name="href">
								#<xsl:value-of select="@name"/>
							</xsl:attribute>
							<small>ת������</small>
						</a>
					</br>
					<br>
						<code>
							(Excel�ļ���<xsl:value-of select="./info/table/@ExcelFile"/>��������<xsl:value-of select="./info/table/@sheet"/>��������<xsl:value-of select="./info/table/@sort"/>)
						</code>
					</br>
					<table border ="1" width="100%">
						<tr>
							<th align="left" bgcolor="Teal">Excel�ֶ���</th>
							<th align="left" bgcolor="Teal">����</th>
							<th align="left" bgcolor="Teal">��С(�ֽ�)</th>
							<th align="left" bgcolor="Teal">�����ֶ���</th>
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
				<!--����������-->
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
							<small>ת������</small>
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