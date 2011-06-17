<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<!-- match the root element -->
	<xsl:template match="/">
		<xsl:apply-templates select="rss" />
	</xsl:template>

<!-- match the rss element -->
	<xsl:template match="rss">
		<xsl:if test="@version != '2.0'">
			<div id="error">Unrecognized RSS feed version.</div>
		</xsl:if>
		<xsl:if test="@version = '2.0'">
			<xsl:apply-templates select="channel" />
		</xsl:if>
	</xsl:template>

<!-- match the channel element -->
	<xsl:template match="channel">
		<ul id="items">
			<xsl:apply-templates select="item" />
		</ul>
		<div id="feed" style="width:200" align="center">
			<a>
				<xsl:attribute name="href"><xsl:value-of select="link" /></xsl:attribute>
				<xsl:attribute name="target">_blank</xsl:attribute>
				<img>
					<xsl:attribute name="src"><xsl:value-of select="image/url" /></xsl:attribute>
					<xsl:attribute name="width"><xsl:value-of select="image/width" /></xsl:attribute>
					<xsl:attribute name="height"><xsl:value-of select="image/height" /></xsl:attribute>
					<xsl:attribute name="title"><xsl:value-of select="image/title" /></xsl:attribute>
					<xsl:attribute name="alt"><xsl:value-of select="image/description" /></xsl:attribute>
					<xsl:attribute name="border">0</xsl:attribute>
				</img>
			</a>
		</div>
	</xsl:template>

<!-- match the item element -->
	<xsl:template match="item">
		<li>
			<a>
				<xsl:attribute name="href"><xsl:value-of select="link" /></xsl:attribute>
				<xsl:attribute name="target">_blank</xsl:attribute>
				<xsl:value-of select="title" disable-output-escaping="yes" />
			</a>
		</li>
	</xsl:template>

</xsl:stylesheet>