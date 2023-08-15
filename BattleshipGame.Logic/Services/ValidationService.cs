using BattleshipGame.API.Models.Game;

namespace BattleshipGame.Logic.Services
{
    public class ValidationService : IValidationService
    {
        public bool OneFieldShipValidation(Field selectedField, List<Field> allFields)
        {
            if (!selectedField.IsEmpty || !selectedField.IsValid)
            {
                return false;
            }

            // Takes selected field and marked as occupied if validation is true.

            selectedField.IsEmpty = false;
            selectedField.ShipSize = 1;

            // Finds invalid fields and adjusts IsValid property to false.

            var invalidFields = allFields.FindAll(xy => (xy.X == selectedField.X + 1 || xy.X == selectedField.X - 1) && xy.Y == selectedField.Y
            || (xy.Y == selectedField.Y - 1 || xy.Y == selectedField.Y + 1) && xy.X == selectedField.X
            || xy.X == selectedField.X - 1 && xy.Y == selectedField.Y - 1
            || xy.X == selectedField.X - 1 && xy.Y == selectedField.Y + 1
            || xy.X == selectedField.X + 1 && xy.Y == selectedField.Y - 1
            || xy.X == selectedField.X + 1 && xy.Y == selectedField.Y + 1
            && xy.IsValid == true
            && xy.IsEmpty == true)
                .ToList();

            foreach (var field in invalidFields)
            {
                field.IsValid = false;
            }

            return true;
        }

        public bool TwoFieldShipValidation(List<Field> selectedFields, List<Field> allFields)
        {
            foreach (var field in selectedFields)
            {
                if (!field.IsValid || !field.IsEmpty) return false;
            }

            foreach (var field in selectedFields)
            {
                field.IsEmpty = false;
                field.ShipSize = 2;
            }

            // Finds invalid fields and adjusts IsValid property to false.

            var invalidFields = new List<Field>();

            invalidFields = allFields.FindAll(xy =>
            ((Math.Abs(xy.X - selectedFields[0].X) <= 1 && Math.Abs(xy.Y - selectedFields[0].Y) <= 1) &&
                xy.X >= 0 && xy.Y >= 0 && xy.X < 10 && xy.Y < 10) ||
            ((Math.Abs(xy.X - selectedFields[1].X) <= 1 && Math.Abs(xy.Y - selectedFields[1].Y) <= 1) &&
                xy.X >= 0 && xy.Y >= 0 && xy.X < 10 && xy.Y < 10))
            .ToList();

            foreach (var field in invalidFields)
            {
                field.IsValid = false;
            }

            return true;
        }
    }
}
