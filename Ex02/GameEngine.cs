﻿using System;
using Ex02;

namespace Engine
{
    public class GameEngine
    {
        private GameBoard m_GameBoard=null;
        private bool m_IsStillPlaying;
        private Player m_FirstPlayer = null, m_SecondPlayer = null, m_CurrentTurnPlayer;
        private Random m_RandomNumberGenerator;
        private bool m_IsGameStarted;
        public GameEngine()
        {
            m_IsStillPlaying = true;
            m_RandomNumberGenerator = new Random();
        }

        public void Create2Players(ePlayerName i_FirstPlayerName, ePlayerName i_SecondPlayerName)
        {
            m_FirstPlayer = new Player(i_FirstPlayerName,eBoardCellValue.X);
            m_SecondPlayer = new Player(i_SecondPlayerName,eBoardCellValue.O);
        }
        public eBoardSizeError CreateNewEmptyGameBoard(ushort i_BoardSize)
        {
            eBoardSizeError sizeStatus;

            if(i_BoardSize < (ushort)eBoardSizeError.MinSize)
            {
                sizeStatus = eBoardSizeError.MinSize;

            }
            else if(i_BoardSize > (ushort)eBoardSizeError.MaxSize)
            {
                sizeStatus = eBoardSizeError.MaxSize;
            }
            else
            {
                sizeStatus = eBoardSizeError.Valid;
                m_GameBoard = new GameBoard(i_BoardSize);
                m_IsGameStarted = false;
            }

            return sizeStatus;
        }

        public ePlayerName GetCurrentTurnPlayerName()
        {
            return m_CurrentTurnPlayer.Name;
        }

        public bool IsValidMoveInTurn(MoveData i_Data)
        {
            return m_GameBoard.IsValidAndEmptyCell(i_Data.SelectedRow, i_Data.SelectedRow)
                   && i_Data.CellValue != eBoardCellValue.Empty;
        }

        public eBoardCellValue[,] GetCurrentBoardState()
        {
            return m_GameBoard.GetCurrentBoardState();
        }

        private void switchCurrentPlayerToOtherPlayer()
        {
            if(m_CurrentTurnPlayer == m_FirstPlayer)
            {
                m_CurrentTurnPlayer = m_SecondPlayer;
            }
            else
            {
                m_CurrentTurnPlayer = m_FirstPlayer;
            }
        }

        private void checkIfaPlayerWinSession()
        {
            bool isPlayerWinSession= m_GameBoard.IsBoardHaveAnyRowColumnDiagonalFilled(m_CurrentTurnPlayer.GameSymbol);

            switchCurrentPlayerToOtherPlayer();
            if(isPlayerWinSession)
            {
                m_CurrentTurnPlayer.incrementGameSessionsScore();
            }
        }
        public bool MakeValidGameMoveForCurrentPlayer(int i_Row,int i_Column)
        {
            MoveData currentMoveData = new MoveData((ushort)i_Row, (ushort)i_Column, m_CurrentTurnPlayer.GameSymbol);
            bool isValidMove = IsValidMoveInTurn(currentMoveData);

            if (isValidMove)
            {
                m_GameBoard.ChangeValueIfEmptyCell(currentMoveData);
                checkIfaPlayerWinSession();
            }

            return isValidMove;
        }
        public eStartingGameStatus ValidateInitializationGameParameters()
        {
            eStartingGameStatus gameInitializationStatus;
     
            if (m_FirstPlayer == null || m_SecondPlayer == null)
            {
                gameInitializationStatus = eStartingGameStatus.NotChooseTwoPlayerForTheGame;
            }
            else if(m_GameBoard == null)
            {
                gameInitializationStatus = eStartingGameStatus.NotChooseGameBoard;
            }
            else
            {
                gameInitializationStatus = eStartingGameStatus.StartSuccessfully;
            }
            m_CurrentTurnPlayer = m_FirstPlayer;

            return gameInitializationStatus;
        }

    }
}