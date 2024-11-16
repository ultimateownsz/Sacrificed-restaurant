<!--
REMINDER:
- Complete all sections labeled as (MUST).
- Only leave a checkbox unchecked if it doesn’t apply or needs improvement.
- Use the "Code Review Explanation" section to explain any unchecked items.
- (FOR REVIEWERS) ~ Review the "Final Review Checklist" to confirm everything is ready for merge.
-->

## Types of Changes
<!--
INSTRUCTIONS (FOR PULL REQUESTER):
- Check only the boxes that apply to the changes made in this pull request.
- Select multiple types if applicable, but be mindful to label breaking changes clearly
-->
**Tip**: Select all the types of changes that apply to this pull request. Be sure to mark breaking changes if they apply.
- [ ] 🛠️ Code optimization / refactoring
- [ ] 🐛 Bug fix (fixes an existing issue)
- [ ] ✨ New feature (adds functionality)
- [ ] ⚠️ Breaking change (modifies existing functionality)

---

## Summary [PULL REQUESTER MUST]
<!-- 
INSTRUCTIONS (FOR PULL REQUESTER):
- Briefly describe the purpose of this pull request. Explain what it implements, enhances, or fixes.
- If this PR is related to an issue, mention the issue number (e.g., "Fixes #123").
- Keep this summary concise but informative.
-->

**Summary Of Changes**: _(e.g., "This PR refactors the validation logic in the Presentation Layer to improve error handling.")_

---

## Pull Request Checklist [PULL REQUESTER MUST]
<!-- 
INSTRUCTIONS (FOR PULL REQUESTER):
- Check each box to confirm you have completed these steps before submitting your pull request.
- This checklist ensures the quality and readiness of the pull request for review.
-->
**Tip**: Use this checklist to confirm you’ve completed essential steps before submitting for review. Checking each box ensures quality and readiness for review
- [ ] ✅ I have tested my code, and it works as expected.
- [ ] 📝 I added a description explaining the changes.
- [ ] 🔍 I reviewed my own code for clarity and quality.

---

## Layer Specific Checklists [PULL REQUESTER CHECKS]
<!-- 
INSTRUCTIONS (FOR PULL REQUESTER):
- Use these checklists to verify the quality and completeness of each application layer.
- Mark each item as completed if it has been properly implemented and reviewed.
-->
**Tip**: Use these checklists to verify each application layer has been implemented to standards. Mark items as completed if they have been reviewed and are correct.
### Presentation Layer Checklist
<details>
<summary> Click to expand the Presentation Layer Checklist</summary>

- [ ] Ensure all user inputs are validated in real time. (Give hints or messages if the format is incorrect)
- [ ] Display user-friendly error messages (e.g., "Please enter date in YYYY-MM-DD format")
- [ ] Allow users to re-enter incorrect information without restarting
- [ ] Provide clear prompts for input formats (dates, phone numbers etc.)
- [ ] Retain valid field entries; clear only invalid fields.
- [ ] Avoid duplicate errors.

</details>

### Logic Layer Checklist
<details>
<summary>Click to expand the Logic Layer Checklist</summary>

- [ ] Implement consistent validation rules and error handling
- [ ] Return actionable error messages for the presentation layer.
- [ ] Pass only validated data to the data access layer.
- [ ] Retain valid data entries; flag only invalid fields.

</details>

### Data Access Layer Checklist
<details>
<summary> Click to expand the Data Access Layer Checklist</summary>

- [ ] Validate data before insertion.
- [ ] Ensure data consistency on insertion.
- [ ] Handle database constraints, providing specific error messages.
- [ ] Commit only fully validated data to the database.

</details>

---

## Code Reviewer Checklist [REVIEWERS ONLY, MUST]
<!-- 
REVIEWER INSTRUCTIONS:
- Only complete this checklist if you are reviewing the code.
- After verifying each point, check the box to confirm the code meets quality standards.
- If any box is left unchecked, please provide an explanation in the "Code Review Explanation" section below.standards.
-->
**Tip**: Reviewers, use this checklist to verify that the code meets quality standards. If any items cannot be checked, please provide an explanation in the "Code Review Explanation" section below.
1. - [ ] 📖 The code is easy to read and understand.
2. - [ ] 🧪 Tests are included for new features.
3. - [ ] 🧹 No unused or commented-out code remains.
4. - [ ] 🚫 No unnecessary log or debug information.
5. - [ ] 🛡️ Errors are properly handled.
6. - [ ] 🔤 Variables and functions are clearly named.
7. - [ ] 💬 Relevant comments have been added to the code for clarity.

### Add Code Review Explanation [REVIEWERS ONLY, REQUIRED IF ANY CHECKS ARE LEFT UNCHECKED]
<!-- 
REVIEWER INSTRUCTIONS:
- Use this section only if any checklist items above are left unchecked.
- - Provide a clear explanation of what could be improved or is missing for each unchecked item.
- Suggest alternatives or solutions if possible. This section is optional if all checklist items are checked.
-->

**If any checklist items are left unchecked, please explain why below:**
<details>
<summary>Example Explanation Format</summary>
- **Checklist Item**: "The code is easy to read and understand."
- **Explanation**: "Several nested conditions make the code difficult to follow. Consider breaking them into helper functions for readability."
</details>

---

**Tip**: Complete this checklist to confirm that all necessary testing, documentation, and preparation for merging is complete.
## Final Review Checklist [BOTH PULL REQUESTER & REVIEWER]
- [ ] All necessary changes have been tested.
- [ ] All required documentation has been updated.
- [ ] This pull request is ready for merge.