const listOfEdit = document.querySelectorAll(".editBtn");

listOfEdit.forEach((btn) => {
    btn.addEventListener("click", (event) => {
        event.preventDefault();
        const row = btn.closest("tr"); // Get the parent row of the button
        const isEditing = btn.textContent.trim() === "Edit";

        if (isEditing) {
            row.innerHTML = `
                <form asp-controller="Trainers" asp-action="RegisterTrainer" method="post" enctype="multipart/form-data">
                    <td data-label="FirstName">@trainer[1]</td>
                    <td data-label="LastName">@trainer[2]</td>
                    <td data-label="Email">@trainer[3]</td>
                    <td data-label="Phone">@trainer[4]</td>
                    <td data-label="Field">@trainer[5]</td>
                    <td><img src="@trainer[6]" alt="Profile Image" style="width: 80px; height: 80px;" /></td>
                        <td>
                            <div class="row mb-2">
                                <form asp-action="DeleteTrainer" asp-route-id="@trainer[0]" method="post" onsubmit="return confirm('Are you sure you want to delete this trainer? ?');">
                                    <button type="submit" class="btn btn-danger btn-sm">
                                        <i class="fa fa-trash"></i> Delete
                                    </button>
                                </form>
                            </div>
                            <div class="row">
                                <form method="post" >
                                    <button type="submit" class="btn btn-primary btn-sm editBtn" id=@trainer[0]>
                                        <i class="fa fa-edit"></i>Edit
                                    </button>
                                </form>
                            </div>
                        </td>
                </form>
            `
            btn.innerHTML = '<i class="fa fa-edit"></i> Save';

            const cells = row.querySelectorAll("td:not(:last-child)"); // Exclude the last cell with buttons
            cells.forEach((cell, index) => {
                const previousValue = cell.innerText.trim();
                const inputType = index === 6 ? "file" : "text"; // Use file input for image, text for others
                cell.innerHTML = `<input type="${inputType}" class="form-control" value="${previousValue}" />`;
                cell.innerHTML = `<input asp-for="FirstName" type="${inputType}" class="form-control" value="${previousValue}"/>
                            <span asp-validation-for="FirstName" class="text-danger"></span>`;
            });
        } else {
            btn.innerHTML = '<i class="fa fa-edit"></i> Edit';
            const inputs = row.querySelectorAll("input");
            inputs.forEach((input) => {
                const cell = input.closest("td");
                const newValue = input.value.trim();
                cell.innerText = newValue; // Replace input with its value
            });
        }
    });
});
