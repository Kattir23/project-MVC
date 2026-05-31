// GET: Insuree/Admin
public ActionResult Admin()
{
    // Retrieve all records from the database to display quotes
    var insurees = db.Insurees.ToList();
    return View(insurees);
}