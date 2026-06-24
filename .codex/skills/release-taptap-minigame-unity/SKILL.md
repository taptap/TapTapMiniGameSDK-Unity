---
name: release-taptap-minigame-unity
description: Release the TapTap MiniGame Unity SDK from develop to main, including version checks, PR merge, Git tag, and GitHub Release.
---

# TapTap MiniGame Unity SDK Release

Use this skill when releasing this repository from `develop` to `main`.

## Release Shape

This repository publishes Unity package releases from the `main` branch.

The established flow is:

1. Prepare the version on `develop`.
2. Open a GitHub PR from `develop` to `main`.
3. Merge the PR.
4. Tag the resulting `main` merge commit with the package version.
5. Create a GitHub Release from that tag.

Tags are expected to point at `main`, not at the original auto-build commit on `develop`.

## Preflight Checks

Run these checks before creating a PR:

```bash
git status --short --branch
git fetch origin main develop --tags
git diff --stat origin/main..origin/develop
git diff origin/main..origin/develop -- package.json CHANGELOG.md README.md
```

Confirm:

- The working tree is clean or only contains intended release-documentation changes.
- `package.json` has the target package version.
- `CHANGELOG.md` has a top entry for the same target version.
- The release tag does not already exist:

```bash
git ls-remote --tags origin
git tag --list
```

Check GitHub release state:

```bash
gh release list --repo taptap/TapTapMiniGameSDK-Unity --limit 20
```

## Version Conventions

The version source of truth is:

- `package.json` field: `version`
- `CHANGELOG.md` top release section
- Git tag name, for example `2.0.19`
- GitHub Release tag/title, for example `2.0.19`

The changelog follows this shape:

```markdown
## [2.0.19] - 2026-06-01
### Added
1. 支持新版支付接口
```

## Create The Release PR

If no open PR exists from `develop` to `main`, create one:

```bash
gh pr list \
  --repo taptap/TapTapMiniGameSDK-Unity \
  --head develop \
  --base main \
  --state open

gh pr create \
  --repo taptap/TapTapMiniGameSDK-Unity \
  --base main \
  --head develop \
  --title "Develop" \
  --body "2.0.19

支持新版支付接口"
```

Use the actual target version and changelog summary in the PR body.

## Merge The PR

Inspect the PR before merging:

```bash
gh pr view <number> --repo taptap/TapTapMiniGameSDK-Unity
gh pr diff <number> --repo taptap/TapTapMiniGameSDK-Unity --stat
```

Merge with the repository default merge behavior unless the repo UI or maintainers require another method:

```bash
gh pr merge <number> --repo taptap/TapTapMiniGameSDK-Unity --merge
```

After the PR is merged, update local `main`:

```bash
git fetch origin main develop --tags
git switch main
git pull --ff-only origin main
```

## Tag The Main Commit

Create an annotated tag on the updated `main` HEAD:

```bash
git tag -a 2.0.19 -m "2.0.19"
git push origin 2.0.19
```

Use the actual target version. Do not tag `develop` directly.

## Create GitHub Release

Recent public releases `2.0.17` and `2.0.18` were normal releases, not prereleases. Create the new release as a normal release unless maintainers explicitly request a prerelease.

```bash
gh release create 2.0.19 \
  --repo taptap/TapTapMiniGameSDK-Unity \
  --target main \
  --title "2.0.19" \
  --notes "2.0.19"
```

If maintainers want richer notes, use the top `CHANGELOG.md` entry instead.

## Post-Release Verification

Verify the final state:

```bash
git ls-remote --tags origin | grep 'refs/tags/2.0.19'
gh release view 2.0.19 \
  --repo taptap/TapTapMiniGameSDK-Unity \
  --json tagName,name,isPrerelease,isDraft,publishedAt,targetCommitish,url
git show --format=fuller --no-patch 2.0.19
```

Confirm:

- The tag resolves to the expected `main` commit.
- The GitHub Release exists and is not a draft.
- The release is not marked as prerelease unless intended.
- `main` contains the same package version and changelog entry.

