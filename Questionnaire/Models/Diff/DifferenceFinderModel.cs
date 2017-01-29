using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models
{
    public class DifferenceFinderModel
    {
        public DiffPaneModelParser OldText { get; set; }
        public DiffPaneModelParser NewText { get; set; }

        public static implicit operator DifferenceFinderModel(SideBySideDiffModel diffModel)
        {
            return new DifferenceFinderModel
            {
                NewText = diffModel.NewText,
                OldText = diffModel.OldText
            };
        }
    }

    public class DiffPaneModelParser
    {
        public List<DiffPieceModel> Lines { get; set; }

        public static implicit operator DiffPaneModelParser(DiffPaneModel model)
        {
            return new DiffPaneModelParser
            {
                Lines = model.Lines == null ? null : model.Lines.Select(x => (DiffPieceModel)x).ToList()
            };
        }
    }

    public class DiffPieceModel
    {
        public int? Position { get; set; }
        public string Text { get; set; }
        public List<DiffPieceModel> SubPieces { get; set; }
        public ChangeType Type { get; set; }

        public static implicit operator DiffPieceModel(DiffPiece diffPiece)
        {
            return new DiffPieceModel
            {
                Position = diffPiece.Position,
                Text = diffPiece.Text,
                Type = diffPiece.Type,
                SubPieces = diffPiece.SubPieces == null ? null : diffPiece.SubPieces.Select(x => (DiffPieceModel)x).ToList()
            };
        }
    }
}
