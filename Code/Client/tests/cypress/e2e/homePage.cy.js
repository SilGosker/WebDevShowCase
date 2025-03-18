describe('template spec', () => {
  it('passes', () => {
    cy.visit('http://localhost:53782/');

    cy.get("h1").contains("Sil Gosker");

    cy.contains("Inloggen").click();

    cy.get("input[type=email]")
      .click()
      .type("test@example.com");

    cy.get("input[type=password]")
      .click()
      .type("not-my-password");

    cy.get("button[type=submit]").click();

    cy.contains("Kon niet inloggen");
  });
});