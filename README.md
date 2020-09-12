# ShapeServer

:book: Тестовое задание


## Общие идеи решения задачи

1. Раз параметры фигуры могут меняться, значит может меняться её тип: изменив длину прямоугольника можно получить квадрат
2. Раз тип может менять сделать его вычислимым полем в БД. На вход принимать параметры фигуры в виде массива. 
   Один параметр - тип круг, два и они не равны - прямоугольник, два и равны - квадрат, что-то другое - неизвестная фигура
3. Дерево хранить в таблице Shape, 1 строка = 1 узел, ParentId - ссылка на Id родителя, TreePath - вычисляемое поле, путь к данному узлу от корня дерева
4. Хранить отдельно суррогатный Id базы и внешний идентификатор узла Identifier
5. Сделать таблицу ShapeEJRecord где создавать запись каждый раз когда изменяются параметры фигуры в таблице Shape, связать ShapeEJRecord с Shape по внешнему ключу
6. Доступ к Shape через swagger api - полный, к ShapeEJRecord - только для чтения

### Запросы
	
	Запросы должны возвращать идентификатор объекта, его тип и площадь на указанную, в виде параметра SQL запроса, дату
	
    a. Все объекты дерева, упорядоченные от корня
	Выставляем TreePath в порядке возрастания количества узлов. Чем больше узлов - тем дальше от корня

	PREPARE getOrderedShapes (timestamp, timestamp) AS
	SELECT s1."Identifier",s1."ShapeType", sj."CurrentArea"
	FROM "Shape" as s1
	JOIN "ShapeEJRecord" as sj ON s1."Id" = sj."ShapeId" 
	AND sj."UpdatedAt">$1
	AND sj."UpdatedAt"<$2
	ORDER BY cardinality(regexp_split_to_array("TreePath",'/'));
   
    b. Все объекты типа круг, у которых родителем является квадрат
	Используем JOIN таблицы к самой себе

	PREPARE getSquareParentsForCircles (timestamp, timestamp) AS
	SELECT s1."Identifier",s1."TreePath",s1."ShapeType",sj."CurrentArea"
	FROM "Shape" as s1
	JOIN "Shape" as s2 ON s1."ParentId" = s2."Id" AND s1."ShapeType" = 0 AND s2."ShapeType" = 1
	JOIN "ShapeEJRecord" as sj ON s1."Id" = sj."ShapeId" 
	AND sj."UpdatedAt">$1
	AND sj."UpdatedAt"<$2


