﻿using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Drawing;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("RemapPal")]
    public class RemapPal : StateController
    {
        private Expression m_source;
        private Expression m_dest;

        public RemapPal(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_source = textSection.GetAttribute<Expression>("source", null);
                m_dest = textSection.GetAttribute<Expression>("dest", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var source = EvaluationHelper.AsVector2(character, m_source, new Vector2(-1, 0));
            var dest = EvaluationHelper.AsVector2(character, m_dest, new Vector2(-1, 0));

            if (source.x == -1)
            {
                source = new Vector2(1, 1);
            }

            if (dest.y < 0)
            {
                return;
            }

            if (source.x < 0 || source.y < 0 || dest.x < 0 || dest.y < 0)
            {
                return;
            }

            PaletteId idSource = new PaletteId((int)source.x, (int)source.y);
            bool sourceOk = character.PaletteList.PalTable.TryGetValue(idSource, out int sourceIndex);
            if (!sourceOk || sourceIndex < 0)
            {
                Debug.LogWarningFormat("Has no dest palette for RemapPal: %v,%v", idSource.Group, idSource.Number);
                return;
            }

            PaletteId idDest = new PaletteId((int)dest.x, (int)dest.y);
            bool destOk = character.PaletteList.PalTable.TryGetValue(idDest, out int destIndex);
            if (!destOk || destIndex < 0)
            {
                Debug.LogWarningFormat("Has no dest palette for RemapPal: %v,%v", idDest.Group, idDest.Number);
                return;
            }


            if (idSource.Group != idDest.Group)
            {
                UnityEngine.Debug.LogWarningFormat("The source and destination color palette do not have the same color depth for [Source RemapPal: %v,%v] [Dest RemapPal: %v,%v]", idSource.Group, idSource.Number, idDest.Group, idDest.Number);
                return;
            }

            if (dest.x == -1)
            {
                character.PaletteList.PalTex[sourceIndex] = character.PaletteList.PalTexBackup[sourceIndex];
                return;
            }

            character.PaletteList.PalTex[sourceIndex] = character.PaletteList.PalTexBackup[destIndex];

        }
    }
}