[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
{
    if (ModelState.IsValid)
    {
        // a. Start with a base of $50 / month.
        decimal monthlyTotal = 50m;

        // Calculate age based on DateOfBirth
        int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
        if (insuree.DateOfBirth.Date > DateTime.Now.AddYears(-age)) age--;

        // b, c, d. Age logic guidelines
        if (age <= 18)
        {
            monthlyTotal += 100;
        }
        else if (age >= 19 && age <= 25)
        {
            monthlyTotal += 50;
        }
        else // 26 or older
        {
            monthlyTotal += 25;
        }

        // e. If the car's year is before 2000
        if (insuree.CarYear < 2000)
        {
            monthlyTotal += 25;
        }

        // f. If the car's year is after 2015
        if (insuree.CarYear > 2015)
        {
            monthlyTotal += 25;
        }

        // g & h. Car Make and Model logic (Porsche / 911 Carrera)
        if (insuree.CarMake != null && insuree.CarMake.ToLower() == "porsche")
        {
            monthlyTotal += 25; // Add $25 because it's a Porsche

            if (insuree.CarModel != null && insuree.CarModel.ToLower() == "911 carrera")
            {
                monthlyTotal += 25; // Add an additional $25 for the 911 Carrera (Totaling $50)
            }
        }

        // i. Add $10 to the monthly total for every speeding ticket
        monthlyTotal += (insuree.SpeedingTickets * 10);

        // j. If the user has ever had a DUI, add 25% to the total
        if (insuree.DUI)
        {
            monthlyTotal += (monthlyTotal * 0.25m);
        }

        // k. If it's full coverage, add 50% to the total
        // Note: Change "Full Coverage" if your application saves this value differently (like a boolean or distinct string)
        if (insuree.CoverageType == "Full" || insuree.CoverageType == "Full Coverage")
        {
            monthlyTotal += (monthlyTotal * 0.50m);
        }

        // Assign the final calculated total to the Quote field
        insuree.Quote = monthlyTotal;

        db.Insurees.Add(insuree);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    return View(insuree);
}