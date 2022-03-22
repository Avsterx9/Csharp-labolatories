# Csharp-labolatories


## 4 sets of exercises to practice c#

## Set I

### Ex1
Design an abstract class / record that defines the area, perimeter and returns the lengths of the sides of the triangle (any). The classes inheriting from this class are to be a triangle:
* equilateral (constructor with one parameter),
* isosceles (constructor with two parameters - two different sides),
* rectangular (constructor with two parameters - leg lengths).

> Overload the `ToString ()` method to contain the basic information set. The task can be completed in two ways:
1.Implement the methods for calculating the area and perimeter in an abstract class using the formula


$
	P =\sqrt{p\cdot(p-A)\cdot(p-B)\cdot(p-C)}
$

### Exercise 2

Create a generic class for employee files that allows you to:

* adding / removing,
* display,
* validation of existing employees,
* data search.

The employee class must contain at least 5 attributes. Every employee should have a workplace. The class should contain data validation (via the `Validate ()` method). The employee class should contain the methods `Show ()` and `IsMatch ()` that the directory will use when searching.

The `Format ()` method can help with the display.

### Exercise 3

Add to the file from the first task reading and writing data in TXT, XML and JSON format (minimum one). Use the builder design pattern for this. The abstraction of a particular record can be passed to the directory constructor, which has a `Save ()` method.

### Exercise 4
 
Extend the previous task with the ability to encrypt data with the Caesar method. The shift block should be larger than one. In order to store it, use the `ConfigurationManager` class (first add a reference to the` System.Configuration` library).

## Set II

### Exercise 1

Write a function (it is best to redefine `ToString`), which will return a string in the form of ONP for a given expression in the C # programming language. For example:

for the expression `x = a - b * c`, where` a`, `b`,` c` are classes of type `OnpExpression`, and` -`, `*` are operators, function `x.ToString ()` will return "ab - c *" (no priority for the operation and no parentheses).

The `OnpExpression` class should store its values ​​to make it easy for the` ToString` method to return a result.

## Exercise 2

Write an operator for `<'and`> `that checks whether the sum of the items in a list is greater or less than the second list. For example, for `a = [1,2,3,4]` and `b = [20,30]` a <b` should return `true`.

### Exercise 3

There is a class `Student` that stores the following data: index number, age, gender, year of study, semester; the `Degree` class stores data: subject, grade, year of credit, semester. Using * LINQ *.

* Combine data from the classes `Student` and` Degree`.
* View students whose age is above the average of the students per year.
* View students whose grade point average is greater than that of other students in the year.

## Set III

### Exercise 1
Implement Builder pattern

### Exercise 2
Implement Prototype pattern

### Exercise 3
Implement the class `SortedList` from the strategy design pattern.

###Exercise 4
Implement an observer class for the selected collection. When a property value changes, display what has changed on the console.


## Set IV


### Exercise 1

Write a `CRUD` program that collects signals (events) from various sources. The table should have:

- the exact date of the event,
- source of the event,
- event type (information, warning, error, fatal error),
- Additional data,
- identifier (the address can be e.g. IP).

An index should be placed on the event type column, and the values ​​should be limited to those listed in parentheses.

### Exercise 2

To task 2 in Lab 1, add database support.

### Exercise 3

Write a class for logging errors. Events should be logged to the database as * bulk load *. The class should support two databases - main (any relational database) and local (for temporary data). The login scheme is as follows:
- if the (operational) database is not available, login should be made to the local database,
- if the connection to the main database is restored, the class should transfer all data from the local database to the main database.

Be aware of the uniqueness of the primary key.
