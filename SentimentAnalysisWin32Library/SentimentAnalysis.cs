﻿using System;
using System.Collections.Generic;

namespace kfouwels.lib.SentimentAnalysis
{
    public class SentimentAnalyser
    {
        private readonly Dictionary<string, sbyte> _intensifiers;
        private readonly Dictionary<string, sbyte> _inverters;
        private readonly Dictionary<string, sbyte> _wordlist;

        public SentimentAnalyser(Dictionary<string, sbyte> wordlistIn, Dictionary<string, sbyte> invertersIn,
                                 Dictionary<string, sbyte> intensifiersIn)
        {
            _wordlist = wordlistIn;
            _inverters = invertersIn;
            _intensifiers = intensifiersIn;
        }

        public decimal Analyse(string[] word)
        {
            decimal sentimentValue = 0; //Master value of awesomeness
            decimal wordsFound = 0;
            decimal v; //Temp value for calculations
            decimal w; //Temp value for calculations

            for (int wordCycler = 0; wordCycler < word.Length; wordCycler++) //Cycle word
            {
                Console.WriteLine("processing {0}", word[wordCycler]);
                if (_wordlist.ContainsKey(word[wordCycler]))
                {
                    wordsFound++;
                    if (_inverters.ContainsKey(word[wordCycler - 1]))
                    {
                        if (_intensifiers.ContainsKey(word[wordCycler - 2]))
                        {
                            //intensifiers - inverters - word
                            w = _wordlist[word[wordCycler]];
                            v = _intensifiers[word[wordCycler]];

                            sentimentValue += ((w + (w*v/100))*-1);
                        }
                        else
                        {
                            //inverters - word
                            sentimentValue += (_wordlist[word[wordCycler]]*-1); // -1 inverts
                        }
                    }
                    else
                    {
                        if (_intensifiers.ContainsKey(word[wordCycler - 1]))
                        {
                            if (_inverters.ContainsKey(word[wordCycler - 2]))
                            {
                                //inverters - intensifiers - word

                                w = _wordlist[word[wordCycler]];
                                v = _intensifiers[word[wordCycler]];

                                sentimentValue += ((w + (w*v/100))*-1);
                            }
                            else
                            {
                                // intensifiers - word
                                //+= (w + (w * v /100))

                                w = _wordlist[word[wordCycler]];
                                v = _intensifiers[word[wordCycler]];

                                sentimentValue += (w + (w*v/100));
                            }
                        }
                        else
                        {
                            //word
                            sentimentValue += _wordlist[word[wordCycler]];
                        }
                    }
                }
            } // end For

            return (sentimentValue/wordsFound);
        }
    }
}