using BattleshipGame.API.Models.Game;

namespace BattleshipGame.Logic.Services
{
    public class ValidationService : IValidationService
    {
        public bool OneFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields, string username)
        {
            var selectedField = allFields.FirstOrDefault(xy => xy.X == startX && xy.Y == startY);

            if (!selectedField.IsEmpty || !selectedField.IsValid)
            {
                return false;
            }

            // Takes selected field and marked as occupied if validation is true.

            selectedField.IsEmpty = false;
            selectedField.ShipSize = 1;
            selectedField.Username = username;

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

        public bool TwoFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields, string username)
        {
            var selectedFields = allFields.FindAll(f => f.X == startX && f.Y == startY
               || f.X == endX && f.Y == endY)
                   .ToList();

            if (selectedFields.Count != 2) return false;

            foreach (var field in selectedFields)
            {
                if (!field.IsValid || !field.IsEmpty) return false;
            }

            foreach (var field in selectedFields)
            {
                field.IsEmpty = false;
                field.ShipSize = 2;
                field.Username = username;
            }

            // Finds invalid fields and adjusts IsValid property to false.

            var invalidFields = new List<Field>();

            invalidFields = allFields.FindAll(xy =>
            ((Math.Abs(xy.X - selectedFields[0].X) <= 1 && Math.Abs(xy.Y - selectedFields[0].Y) <= 1)
            && xy.X >= 0 && xy.Y >= 0 && xy.X < 10 && xy.Y < 10)
            || ((Math.Abs(xy.X - selectedFields[1].X) <= 1 && Math.Abs(xy.Y - selectedFields[1].Y) <= 1)
            && xy.X >= 0 && xy.Y >= 0 && xy.X < 10 && xy.Y < 10))
                .ToList();

            foreach (var field in invalidFields)
            {
                field.IsValid = false;
            }

            return true;
        }

        public bool ThreeFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields, string username)
        {
            var selectedFields = allFields.FindAll(f => (f.X == startX && f.Y == startY)
                || f.X == endX && f.Y == endY
                || f.X == ((startX + endX) / 2) && f.Y == endY
                || f.Y == ((startY + endY) / 2) && f.X == endX)
                    .ToList();

            if (selectedFields.Count != 3) return false;

            foreach (var field in selectedFields)
            {
                if (!field.IsValid || !field.IsEmpty) return false;
            }

            foreach (var field in selectedFields)
            {
                field.IsEmpty = false;
                field.ShipSize = 3;
                field.Username = username;
            }

            var invalidFields = new List<Field>();

            invalidFields = allFields.FindAll(xy =>
            ((Math.Abs(xy.X - selectedFields[0].X) <= 1 && Math.Abs(xy.Y - selectedFields[0].Y) <= 1)
            && (xy.X != selectedFields[0].X || xy.Y != selectedFields[0].Y)) 
            || ((Math.Abs(xy.X - selectedFields[1].X) <= 1 && Math.Abs(xy.Y - selectedFields[1].Y) <= 1)
            && (xy.X != selectedFields[1].X || xy.Y != selectedFields[1].Y)) 
            || ((Math.Abs(xy.X - selectedFields[2].X) <= 1 && Math.Abs(xy.Y - selectedFields[2].Y) <= 1)
            && (xy.X != selectedFields[2].X || xy.Y != selectedFields[2].Y)))
                .ToList();

            foreach (var field in invalidFields)
            {
                field.IsValid = false;
            }

            return true;
        }

        public bool FourFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields, string username)
        {
            var selectedFields = new List<Field>();

            for (int x = Math.Min(startX, endX); x <= Math.Max(startX, endX); x++)
            {
                for (int y = Math.Min(startY, endY); y <= Math.Max(startY, endY); y++)
                {
                    var field = allFields.FirstOrDefault(f => f.X == x && f.Y == y);
                    if (field != null)
                    {
                        selectedFields.Add(field);
                    }
                }
            }

            if (selectedFields.Count != 4) return false;

            foreach (var field in selectedFields)
            {
                if (!field.IsValid || !field.IsEmpty) return false;
            }

            foreach (var field in selectedFields)
            {
                field.IsEmpty = false;
                field.ShipSize = 4;
                field.Username = username;
            }

            var invalidFields = new List<Field>();

            foreach (var field in selectedFields)
            {
                var adjacentFields = allFields.FindAll(xy =>
                    Math.Abs(xy.X - field.X) <= 1 && Math.Abs(xy.Y - field.Y) <= 1
                    && (xy.X != field.X || xy.Y != field.Y)
                    && (xy.IsValid == true && xy.IsEmpty == true))
                    .ToList();

                if (adjacentFields.Count > 0)
                {
                    invalidFields.AddRange(adjacentFields);
                }
            }

            foreach (var field in invalidFields)
            {
                field.IsValid = false;
            }

            return true;
        }
    }
}
