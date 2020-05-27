<scenario name="Test Scenario">
<objective type="Reach" string="Get to the point">
		<checkpoint x="40" y="40" z="0"/>
		<enemyspawn type="CAamu" amount="69" x="0" y="0"/>
	</objective>
	<objective type="Survival" string="Remain alive for {0}">
		<timer time="240"/>
		<enemyspawn type="CAamu" amount="69" x="0" y="0"/>
	</objective>
	<objective type="Elimination" string="Eliminate the target">
		<target type="CNatsa" x="20" y="40" z="20"/>
		<enemyspawn type="CAamu" amount="69" x="0" y="0"/>
	</objective>
	<objective type="Event" string="A textbox should appear">
		<bubble speaker="Test">
			<line soundbyte="dialog_test_01">Tomaattirausku69 elaa rauhassa meren syvyyksissa.</line>
			<line soundbyte="dialog_test_02">ABCDEFGHIJKLMNOPQRSTUVWXYZ !?</line>
			<line soundbyte="dialog_test_03">abcdefghijklmnopqrstuvwxyz 1234567890</line>
		</bubble>
	</objective>
</scenario>