using CapaEntidad;
using Moq;

namespace AulaDigital.Test.NotebooksTests;

public class NotebookCNTests : IClassFixture<FixtureNotebook>
{
    private readonly FixtureNotebook fixture;

    public NotebookCNTests(FixtureNotebook fixture)
    {
        this.fixture = fixture;
    }

    #region INSERT NOTEBOOK Transaccion
    [Fact]
    public void InsertNotebook()
    {
        int idUsuario = 2;
        Notebooks? nuevo = fixture.CreateNotebook(0);

        fixture.RepoNotebooks.Setup(r => r.GetById(nuevo.IdElemento)).Returns((Notebooks?)null);

        Assert.Throws<Exception>(() => fixture.NotebooksCN.CrearNotebook(nuevo, idUsuario));

        fixture.RepoNotebooks.Verify(r => r.Insert(It.IsAny<Notebooks>()), Times.Never);
        fixture.MockUow.Verify(u => u.Commit(), Times.Never);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Once);
    }
    #endregion
}
