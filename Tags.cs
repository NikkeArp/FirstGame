
/// <summary>
/// Tags class allows easy tag checking with Unity's 
/// shitty tag system. 
/// </summary>
public static class Tags
{
    /// <summary>
    /// Separator between tags.
    /// </summary>
    /// <value>Separator between tags.</value>
    public static char[] separator = {' '};

    /// <summary>
    /// Gets gameobject's tags in string array.
    /// </summary>
    /// <param name="tag">Gameobject's tag</param>
    /// <returns>Gameobject's tags in string array.</returns>
    public static string[] GetTags(string tag)
    {
        // Separate tags using static separator.
        string[] tags = tag.Split(separator);
        return tags;
    }

    /// <summary>
    /// Gets gameobject's tags in string array.
    /// </summary>
    /// <param name="tag">Gameobjects tag</param>
    /// <param name="separator">Separator between tags</param>
    /// <returns>Gameobject's tags in string array.</returns>
    public static string[] GetTags(string tag, string separator)
    {
        // Separate tags using separator specified in parameters.
        char[] charSeparator = separator.ToCharArray();
        string[] tags = tag.Split(charSeparator);

        return tags;
    }

    /// <summary>
    /// Checks if string of tags contains tag as
    /// substring separated by space-char.
    /// </summary>
    /// <param name="tags">Tags as one string</param>
    /// <param name="tag">Tag that is looked for</param>
    /// <param name="separator">Separator between tags</param>
    /// <returns>True if tag is found inside tags-string</returns>
    public static bool TagsContainTag(string tags, string tag, string separator)
    {
        // Separate tags using separator specified in parameters.
        char[] charSeparator = separator.ToCharArray();
        string[] tagsArray = tags.Split(charSeparator);

        // Go through all the tags until matching tag is found.
        for (int i = 0; i < tagsArray.Length; i++)
        {
            if (tagsArray[i] == tag)
            {
                return true;
            }
        }
        // Tag didn't match any of the tags.
        return false;
    }

    /// <summary>
    /// Checks if string of tags contains tag as
    /// substring separated by space-char.
    /// </summary>
    /// <param name="tags">Tags as one string</param>
    /// <param name="tag">Tag that is looked for</param>
    /// <returns>True if tag is found inside tags-string</returns>
    public static bool TagsContainTag(string tags, string tag)
    {
        // Separate tags using static separator.
        string[] tagsArray = tags.Split(separator);

        // Go through all the tags until matching tag is found.
        for (int i = 0; i < tagsArray.Length; i++)
        {
            if (tagsArray[i] == tag)
            {
                return true;
            }
        }
        // Tag didn't match any of the tags.
        return false;
    }

    /// <summary>
    /// Checks if an array of tags contain specified
    /// tag.
    /// </summary>
    /// <param name="tags">Tag array</param>
    /// <param name="tag">Tag looked for.</param>
    /// <returns>
    /// True if Tag array contains specified tag,
    /// else false.
    /// </returns>
    public static bool TagsContainTag(string[] tags, string tag)
    {
        // Go through all the tags until matching tag is found.
        for (int i = 0; i < tags.Length; i++)
        {
            if (tags[i] == tag)
            {
                return true;
            }
        }
        // Tag didn't match any of the tags.
        return false;
    }

    /// <summary>
    /// Checks if tag array contains atleast one
    /// of the tags looked for.
    /// </summary>
    /// <param name="allTags">All the tags.</param>
    /// <param name="tagsLookedFor">Tags looked for.</param>
    /// <returns>True if atleas one tag is found, else false.</returns>
    public static bool TagsContainOneTag(string allTags, params string [] tagsLookedFor)
    {
        // Separate tags with static separator
        string[] tags = allTags.Split(separator);

        // Go through all separated tags until atleast
        // one of the tags looked for matches currently iterated tag.
        for (int i = 0; i < tags.Length; i++)
        {
            for (int j = 0; j < tagsLookedFor.Length; j++)
            {
                // Tag is found.
                if (tags[i] == tagsLookedFor[j])
                {
                    return true;
                }
            }
        }
        // Tag not found.
        return false;
    }

    /// <summary>
    /// Checks if the string of tags contains all of the
    /// specified tags.
    /// </summary>
    /// <param name="allTags">All tags as one string.</param>
    /// <param name="tagsLookedFor">Tags looked for.</param>
    /// <param name="separator">string that separates tags.</param>
    /// <returns>
    /// Returns true if all the looked for tags
    /// are found in allTags string.
    /// </returns>
    public static bool TagsContainTags(string allTags, char[] charSeparator, params string[] tagsLookedFor)
    {
        // Separate the tags using separator specified in parameters.
        string[] tags = allTags.Split(charSeparator);

        // If the looked for tag amount is larger than all the separated tags,
        // It's impossible to match them all together.
        if (tags.Length < tagsLookedFor.Length)
        {
            return false;
        }

        // Go through all the tags that are looked for.
        for (int i = 0; i < tagsLookedFor.Length; i++)
        {
            // Flag to indicate if inner for-loop found
            // match or not.
            bool foundFlag = false;
            for (int j = 0; j < tags.Length; j++)
            {
                if (tags[j] == tagsLookedFor[i])
                {
                    // Matching tag found
                    foundFlag = true;
                    break;
                }
            }
            // If the inner loop didn't find matching tag
            // return false.
            if (!foundFlag)
            {
                return false;
            }
        }
        // All tags match.
        return true;
    }

    /// <summary>
    /// Checks if the string of tags contains all of the
    /// specified tags.
    /// </summary>
    /// <param name="allTags">All tags as one string.</param>
    /// <param name="tagsLookedFor">Tags looked for.</param>
    /// <returns>
    /// Returns true if all the looked for tags
    /// are found in allTags string.
    /// </returns>
    public static bool TagsContainTags(string allTags, params string[] tagsLookedFor)
    {
        string[] tags = allTags.Split(separator);

        // If the looked for tag amount is larger than all the separated tags,
        // It's impossible to match them all together.
        if (tags.Length < tagsLookedFor.Length)
        {
            return false;
        }

        // Go through all the tags that are looked for.
        for (int i = 0; i < tagsLookedFor.Length; i++)
        {
            // Flag to indicate if inner for-loop found
            // match or not.
            bool found = false;
            for (int j = 0; j < tags.Length; j++)
            {
                if (tags[j] == tagsLookedFor[i])
                {
                    found = true;
                    break;
                }
            }
            // If the inner loop didn't find matching tag
            // return false.
            if (!found)
            {
                return false;
            }
        }
        // All tags match.
        return true;
    }

    /// <summary>
    /// Checks if tag array contains all of the
    /// specified tags.
    /// </summary>
    /// <param name="allTags">All tags in string array.</param>
    /// <param name="tagsLookedFor">Tags looked for.</param>
    /// <returns>
    /// Returns true if all the looked for tags
    /// are found in allTags array.
    /// </returns>
    public static bool TagsContainTags(string[] allTags, params string[] tagsLookedFor)
    {
        // If the looked for tag amount is larger than all the separated tags,
        // It's impossible to match them all together.
        if (allTags.Length < tagsLookedFor.Length)
        {
            return false;
        }

        // Go through all the tags that are looked for.
        for (int i = 0; i < tagsLookedFor.Length; i++)
        {
            // Flag to indicate if inner for-loop found
            // match or not.
            bool found = false;
            for (int j = i; j < allTags.Length; j++)
            {
                if (allTags[j] == tagsLookedFor[i])
                {
                    found = true;
                    break;
                }
            }
            // If the inner loop didn't find matching tag
            // return false.
            if (!found)
            {
                return false;
            }
        }
        // All tags match.
        return true;
    }
}
