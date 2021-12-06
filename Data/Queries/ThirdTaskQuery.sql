USE Task_6

SELECT C.NAME, COUNT(*) as students_count
FROM COURSES as C
INNER JOIN GROUPS ON (GROUPS.COURSE_ID = C.COURSE_ID)
INNER JOIN STUDENTS ON (STUDENTS.GROUP_ID = GROUPS.GROUP_ID)
GROUP BY C.NAME

GO