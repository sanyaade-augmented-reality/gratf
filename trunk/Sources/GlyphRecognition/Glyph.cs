﻿
namespace AForge.Vision.GlyphRecognition
{
    using System;

    public class Glyph : ICloneable
    {
        private string name;
        private byte[,] data;
        private object userData;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Size
        {
            get { return data.GetLength( 0 ); }
        }

        public byte[,] Data
        {
            get { return data; }
            set { data = value; }
        }

        public object UserData
        {
            get { return userData; }
            set { userData = value; }
        }

        public Glyph( string name, int size )
        {
            this.name = name;
            this.data = new byte[size, size];
        }

        public Glyph( string name, byte[,] data )
        {
            this.name = name;
            this.data = data;
        }

        public object Clone( )
        {
            Glyph clone = new Glyph( name, (byte[,]) data.Clone( ) );

            clone.userData = userData;

            return clone;
        }

        public static bool CheckIfRotationInvariant( byte[,] rawGlyphData )
        {
            int size = rawGlyphData.GetLength( 0 );

            if ( size != rawGlyphData.GetLength( 1 ) )
            {
                throw new ArgumentException( "Invalid glyph data array; must be square." );
            }

            int sizeM1 = size - 1;

            bool match2 = true;
            bool match3 = true;
            bool match4 = true;

            for ( int i = 0; i < size; i++ )
            {
                for ( int j = 0; j < size; j++ )
                {
                    byte value = rawGlyphData[i, j];

                    // 180 deg
                    match2 &= ( value == rawGlyphData[sizeM1 - i, sizeM1 - j] );
                    // 90 deg
                    match3 &= ( value == rawGlyphData[sizeM1 - j, i] );
                    // 270 deg
                    match4 &= ( value == rawGlyphData[j, sizeM1 - i] );
                }
            }

            return ( match2 || match3 || match4 );
        }

        public int CheckForMatching( byte[,] rawGlyphData )
        {
            int size = rawGlyphData.GetLength( 0 );

            if ( size != data.GetLength( 0 ) )
                return -1;

            int sizeM1 = size - 1;

            bool match1 = true;
            bool match2 = true;
            bool match3 = true;
            bool match4 = true;

            for ( int i = 0; i < size; i++ )
            {
                for ( int j = 0; j < size; j++ )
                {
                    byte value = rawGlyphData[i, j];

                    // no rotation
                    match1 &= ( value == data[i, j] );
                    // 180 deg
                    match2 &= ( value == data[sizeM1 - i, sizeM1 - j] );
                    // 90 deg
                    match3 &= ( value == data[sizeM1 - j, i] );
                    // 270 deg
                    match4 &= ( value == data[j, sizeM1 - i] );
                }
            }

            if ( match1 )
                return 0;
            else if ( match2 )
                return 180;
            else if ( match3 )
                return 90;
            else if ( match4 )
                return 270;

            return -1;
        }
    }
}
