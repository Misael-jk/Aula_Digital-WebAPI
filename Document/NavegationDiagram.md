```mermaid

flowchart TD
    Login[Login] --> Menu[Menu Principal]

    Menu --> Docentes[Docentes]
    Menu --> Elementos[Elementos]
    Menu --> Carritos[Carritos]
    Menu --> Notebooks[Notebooks]
    Menu --> Categorias[Categorias]
    Menu --> Usuarios[Usuarios]
    Menu --> Prestamos[Prestamos]
    Menu --> Mantenimiento[Mantenimiento]

    Elementos --> HistorialElemento[Historial Elemento]
    Notebooks --> HistorialNotebook[Historial Notebook]
    Carritos --> HistorialCarritos[Historial Carritos]
    Prestamos --> PrestamosDetalle[Detalles del Prestamo]
    Prestamos --> Devolucion[Devolucion]
    Devolucion --> DevolucionDetalle[Detalles de la Devolucion]
    Mantenimiento --> ElementosBajas[Bajas de los Elementos]
    Mantenimiento --> NotebooksBajas[Bajas de las Notebooks]
    Mantenimiento --> CarritosBajas[Bajas de los Carritos]
    Docentes --> DocentesBajas[Bajas de los Docentes]
    Usuarios --> UsuariosBajas[Bajas de los Usuarios]

```