<%@ Control Language="c#" Inherits="OmniPortal.Modules.Blog.Portlets.Calendar" Codebehind="Calendar.ascx.cs" %>
<asp:Calendar id="BlogCalendar" runat="server" DayNameFormat="FirstLetter" CellPadding="1" BorderWidth="1px"
	BackColor="White" Width="100%" ForeColor="Black" Height="200px" Font-Size="8pt" Font-Names="Verdana"
	BorderColor="#A1A1A1" onselectionchanged="BlogCalendar_SelectionChanged">
	<SelectorStyle ForeColor="#336666" BackColor="#99CCCC"></SelectorStyle>
	<NextPrevStyle Font-Size="8pt" Wrap="False" ForeColor="White"></NextPrevStyle>
	<DayHeaderStyle Height="1px" ForeColor="#CC6600" BackColor="#FFC080"></DayHeaderStyle>
	<SelectedDayStyle Font-Bold="True" ForeColor="#CCFF99" BackColor="#009999"></SelectedDayStyle>
	<TitleStyle Font-Size="10pt" Font-Bold="True" Height="25px" BorderWidth="1px" ForeColor="White"
		BorderStyle="Solid" BorderColor="#A1A1A1" BackColor="#A1A1A1"></TitleStyle>
	<WeekendDayStyle BackColor="#CCCCFF"></WeekendDayStyle>
	<OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
</asp:Calendar>
