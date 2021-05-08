using System;

namespace WOLF.Net.Enums.Groups
{
    [Flags]
    public enum Category
    {
        NOT_SPECIFIED = 0,
        [Obsolete("use NOT_SPECIFIED")]
        NotSpecified = NOT_SPECIFIED,

        BUSINESS = 8,
        [Obsolete("use BUSINESS")]
        Business = BUSINESS,

        EDUCATION = 10,
        [Obsolete("use EDUCATION")]
        Education = EDUCATION,

        ENTERTAINMENT = 26,
        [Obsolete("use ENTERTAINMENT")]
        Entertainment = ENTERTAINMENT,

        GAMING = 12,
        [Obsolete("use GAMING")]
        Gaming = GAMING,

        LIFESTYLE = 13,
        [Obsolete("use LIFESTYLE")]
        Lifestyle = LIFESTYLE,

        MUSIC = 14,
        [Obsolete("use MUSIC")]
        Music = MUSIC,

        NEWS_AND_POLITICS = 15,
        [Obsolete("use NEWS_AND_POLITICES")]
        NewsAndPolitics = NEWS_AND_POLITICS,

        PHOTOGRAPHY = 16,
        [Obsolete("use PHOTOGRAPHY")]
        Photography = PHOTOGRAPHY,

        SCIENCE_AND_TECH = 25,
        [Obsolete("use SCIENCE_AND_TECH")]
        ScienceAndTech = SCIENCE_AND_TECH,

        SOCIAL_AND_PEOPLE = 17,
        [Obsolete("use SOCIAL_AND_PEOPLE")]
        SocialAndPeople = SOCIAL_AND_PEOPLE,

        SPORTS = 19,
        [Obsolete("use SPORTS")]
        Sports = SPORTS, 

        TRAVEL_AND_LOCAL = 18,
        [Obsolete("use TRAVEL_AND_LOCAL")]
        TraveAndLocal = TRAVEL_AND_LOCAL,
    }
}
