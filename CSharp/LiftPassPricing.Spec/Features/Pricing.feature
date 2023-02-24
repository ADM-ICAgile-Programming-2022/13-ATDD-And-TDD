Feature: Lift pass pricing calculation
This application solves the problem of calculating the pricing for ski lift passes. There's some intricate logic linked to what kind of 
lift pass you want, your age and the specific date at which you'd like to ski. 

Background:
	Given a baseprice of 35 for 1jour tickets
	And a baseprice of 19 for night tickets

Scenario: Calculate the default pricing for single lift pass
	When the price for a ticket is calculated
	Then a single ticket costs 19

Scenario: Calculate the default pricing for single night-lift pass
	When the price for a night ticket is calculated
	Then a single ticket costs 19

Scenario: Calculate prices and reductions for different ages for daily ticket
	Given an age of
		| age |
		| 5   |
		| 6   |
		| 14  |
		| 15  |
		| 25  |
		| 64  |
		| 65  |
	When the price for a ticket is calculated
	Then it costs
		| cost |
		| 0    |
		| 25   |
		| 25   |
		| 35   |
		| 35   |
		| 35   |
		| 27   |

Scenario: Calculate prices and reductions for different ages for night ticket
	Given an age of
		| age |
		| 5   |
		| 6   |
		| 14  |
		| 15  |
		| 25  |
		| 64  |
		| 65  |
	When the price for a night ticket is calculated
	Then it costs
		| cost |
		| 0    |
		| 25   |
		| 25   |
		| 35   |
		| 35   |
		| 35   |
		| 27   |

Scenario: Calculate prices and reductions for Monday deals
	Given an age and date of
		| age | date       |
		| 15  | 2019-02-22 |
		| 15  | 2019-02-25 |
		| 15  | 2019-03-11 |
		| 65  | 2019-03-11 |
	When the price for a night ticket is calculated
	Then it costs
		| cost |
		| 35   |
		| 35   |
		| 23   |
		| 13   |