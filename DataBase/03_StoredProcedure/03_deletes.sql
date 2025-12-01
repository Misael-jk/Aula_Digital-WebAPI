delimiter $$

drop procedure if exists DeleteCarrito $$
create procedure DeleteCarrito(in unidCarrito tinyint)
begin
	delete 
	from carritos c 
	where idCarrito = unidCarrito;
end $$

delimiter ;



delimiter $$

drop procedure if exists DeleteDocente $$
create procedure DeleteDocente(in unidDocente smallint)
begin
	delete
	from docentes 
	where idDocente = unidDocente;
end $$

delimiter ;