# Orbit Escape — Agent System Prompt

## ON EVERY SESSION START — do this before anything else

1. `cat CHANGELOG.md` — прочти историю: что сделано, что открыто, заметки предыдущего агента
2. `cat Docs/DesignDialogueLog.md` — прочти актуальный дизайн-лог: договорённости, open questions, future-facing идеи
3. `cat Docs/ImplementationRoadmapV2.md` — пойми текущий порядок реализации и чего НЕ надо делать преждевременно
4. `bd ready --json` — найди следующую незаблокированную задачу (OE-MODULE-NNN)
5. `repomix --output .repomix-snapshot.txt --ignore "*.meta,*.unity,Library/**,Temp/**,obj/**"`
6. Прочитай `.repomix-snapshot.txt` — пойми текущее состояние кода
7. `bd update <id> --claim` — возьми задачу в работу
8. Приступай

## AFTER EACH TASK

1. Убедись, что код компилируется без ошибок
2. Обнови snapshot: `repomix --output .repomix-snapshot.txt --ignore "*.meta,*.unity,Library/**,Temp/**,obj/**"`
3. **Обнови CHANGELOG.md** — добавь запись в начало файла (см. формат ниже)
4. `bd close <id> --reason "done"`
5. **Сообщи человеку что задача готова** в следующем формате:

---
✅ **Задача завершена:** `<TASK-ID>` — <название>

📋 **CHANGELOG.md обновлён**

📝 **Закоммить через GitHub Desktop:**
- **Commit title:** `feat(<scope>): <что сделано>`
- **Description:** `<список файлов> — <что делает каждый>`

🎮 **Если в этой задаче менялась сцена или префаб:**
- Открой Unity Editor → **File → Save** (Ctrl+S) → закрой → затем коммит

⏳ Жду подтверждения что закоммитил, затем перехожу к следующей задаче.
---

6. Дождись ответа человека ("готово" / "закоммитил" / "ok")
7. `bd ready --json` → следующая задача

## CHANGELOG FORMAT

Файл: `CHANGELOG.md` в корне проекта. Каждая запись — **в начало файла**.
Task ID берётся из Beads — формат `OE-MODULE-NNN` (OE-CORE-001, OE-PLAYER-002 и т.д.).
Формат машиночитаемый: следующий агент читает CHANGELOG первым делом при входе в проект.

```markdown
## [OE-MODULE-NNN] module/component-name | COMPLETED | YYYY-MM-DD

### context
project: orbit-escape | engine: unity-6 (6000.4.0f1) | pipeline: urp-2d | platform: android

### what_was_implemented
- ComponentName (Assets/Scripts/Layer/Script.cs):
    pattern: EventDrivenSubscriber | SingleResponsibility
    subscribes_to: [GameEvents.OnEventName]
    fires: [GameEvents.OtherEvent]
    responsibility: "one sentence — what this script does and nothing else"

### files
| action   | path                              | notes                        |
|----------|-----------------------------------|------------------------------|
| CREATED  | Assets/Scripts/Layer/Script.cs    | responsibility in one phrase |
| MODIFIED | Assets/Scripts/Core/GameEvents.cs | added OnLanding event        |

### architecture_decisions
- decision: rationale (e.g. "ShakeOffset property: avoids conflict with CameraFollower.LateUpdate")

### known_limitations
- limitation or deferred TODO for next agent

### task_ref
beads_id: OE-MODULE-NNN
commit_title: feat(scope): description
current_branch: feature/OE-MODULE-NNN-short-description

### project_state
completed_tasks: [OE-SETUP-001, OE-CORE-001, ...]
next_ready: OE-MODULE-NNN (из bd ready --json)
blocked: [OE-MODULE-NNN (waiting for OE-MODULE-MMM)]
remaining: N of 22 tasks
current_branch: feature/OE-MODULE-NNN-short-description
```

**Нейминг задач — формат `OE-MODULE-NNN`:**
- Префикс: `OE` — Orbit Escape
- MODULE: `SETUP` / `CORE` / `INPUT` / `PLAYER` / `LEVEL` / `STATE` / `UI` / `AUDIO` / `FEEL` / `BUILD`
- NNN: трёхзначный номер (`001`, `002`...)

Примеры: `OE-CORE-001`, `OE-PLAYER-003`, `OE-BUILD-001`

## AFTER CONTEXT COMPACTION (потеря контекста)

1. `cat CHANGELOG.md` — прочти последнюю запись: поля `project_state.current_branch` и `next_ready` покажут где остановился предыдущий агент
2. `cat Docs/DesignDialogueLog.md` — восстанови текущий дизайн-вектор и открытые вопросы, которые нельзя забыть
3. `cat Docs/ImplementationRoadmapV2.md` — восстанови текущий приоритет реализации
4. `bd list --status=in-progress` — проверь нет ли задачи в статусе "claimed but not closed". Если есть — это незаконченная работа предыдущего агента. Прочти её описание, оцени что было сделано по repomix, продолжи или переоткрой
5. `bd ready --json` — подтверди следующую незаблокированную задачу
6. `cat .repomix-snapshot.txt` — восстанови понимание текущего кода
7. Проверь `current_branch` из CHANGELOG — сообщи человеку: "Я должен работать на ветке `<имя>`. Пожалуйста, убедись что ты переключился на неё в GitHub Desktop"
8. Продолжай с того места, не трогай задачи со статусом COMPLETED

## DESIGN SOURCE OF TRUTH

Для текущего этапа проекта дизайн-источники истины такие:

1. `Docs/DesignDialogueLog.md` — живой лог обсуждений, договорённостей и открытых вопросов
2. `Docs/ImplementationRoadmapV2.md` — актуальный порядок реализации
3. `CHANGELOG.md` — что реально уже сделано в коде и сцене

Если между старым roadmap / beads-задачей / текущим дизайн-логом есть расхождение:
- не игнорируй дизайн-лог
- не перепридумывай направление сам
- сначала сверь это с человеком, если изменение нетривиальное

## ARCHITECTURE RULES (нарушение = перезапись с нуля)

- Вся коммуникация между скриптами ТОЛЬКО через GameEvents.cs
- Запрещено: FindObjectOfType, FindGameObjectWithTag, GetComponent на чужих объектах
- Запрещено: прямые ссылки UI → Player/Gameplay runtime logic (PlayerMovement, PlayerCollider и т.д.)
- Разрешено: UI → orchestration/service классы через `[SerializeField]` (GameStateManager, ScoreManager) — они не runtime gameplay, они сервисы
- Для доступа к Transform игрока — использовать PlayerRegistry.Player (статический класс)
- Целевой размер: ~60 строк. Абсолютный стоп: 80 строк. 60–80 допустимо если одна ответственность.
- Camera logic: ТОЛЬКО в CameraFollower.cs
- Asteroid spawning: ТОЛЬКО в AsteroidSpawner.cs
- UI: четыре Canvas панели в ОДНОЙ сцене (SetActive переключение)
- Подписки на события: ВСЕГДА именованные делегаты (не анонимные лямбды) — иначе нельзя отписаться в OnDestroy

## UNITY SAFETY RULES

- Preserve .meta files — не удаляй и не перемещай без необходимости
- Do not rename or move files unless the task explicitly requires it
- Do not add new Unity packages without clear reason stated in the task
- Do not edit ProjectSettings/ unless the task explicitly requires it
- Be careful with scene and prefab modifications — keep scope tight
- Avoid unnecessary serialization churn
- If a change is risky for scenes, prefabs, or asset references — explicitly mention that risk in the report
- Prefer clear and boring code over clever abstractions
- Keep public API surface small

## CODE STYLE

- Prefer minimal, targeted changes
- Keep classes small — single responsibility
- Use descriptive names (PascalCase public, _camelCase private)
- Avoid creating large frameworks for prototype-only needs

## SELF-CHECK (перед закрытием задачи)

**Компиляция:**
- [ ] Скрипт компилируется без ошибок (агент проверяет синтаксис)?
- [ ] Нет `using` которые не используются?

**Архитектура:**
- [ ] Нет FindObjectOfType / FindGameObjectWithTag?
- [ ] Нет прямых ссылок между Logic и UI скриптами?
- [ ] Нет анонимных лямбд в подписках на события?
- [ ] Каждая подписка в Awake имеет отписку в OnDestroy?
- [ ] Скрипт короче 80 строк? (целевой 60, абсолютный стоп 80)
- [ ] GameEvents.cs не изменён без причины?

**Unity safety:**
- [ ] .meta файлы не затронуты?
- [ ] ProjectSettings/ не изменён без явного требования?
- [ ] Изменения в сцене/префабах минимальны и обоснованы?
- [ ] Если изменял сцену или префаб — в отчёте человеку есть напоминание сохранить (Ctrl+S)?

**Честность:**
- [ ] НЕ заявляй что gameplay проверен в редакторе, если это не так
- [ ] Укажи в CHANGELOG что не было верифицировано

## STOP AND REPORT TO HUMAN IF

- Скрипт вышел за 80 строк И есть явно 2+ ответственности — нет понятного способа разбить
- Ошибка компиляции не устраняется за 2 попытки
- Нужно архитектурное решение, не описанное в документе
- Unity MCP вернул ошибку при создании файла
- Требуется изменить ProjectSettings/ или добавить пакет — сначала спроси
- Нужно переименовать или переместить файл — сначала спроси

## HUMAN CHECKPOINTS (Unity Editor — агент не может его запустить)

После этих задач агент пишет:
```text
🔍 HUMAN CHECKPOINT требуется:
1. Открой Unity Editor
2. Дождись компиляции (нижняя панель Console — нет красных ошибок)
3. Напиши мне: "checkpoint passed" или "errors: [текст ошибок]"
```

Чекпоинты: после OE-CORE-001, OE-PLAYER-003, OE-LEVEL-004, OE-UI-004, OE-FEEL-001

## SKILL USAGE POLICY

Before implementing any non-trivial logic (new script pattern, architecture decision, testing approach), check for applicable skills. Simple tasks (set Inspector value, configure build setting) — skip this step.

**Step 1 — LOCAL skills (Claude Code only, Codex skip to Step 2):**
```bash
ls /mnt/skills/user/        # твои пользовательские скиллы
ls /mnt/skills/public/      # публичные скиллы
```
If a SKILL.md exists for your domain → `cat /mnt/skills/user/<skill>/SKILL.md` → follow its instructions.
Known relevant skills already available: `unity-architecture-specialist`, `coding-agent`, `beads-setup`.

**Step 2 — ONLINE search (both Claude Code and Codex):**
Use `search_prompts` tool from prompts.chat MCP:
```text
search_prompts(query="unity <your topic>", limit=5)
search_prompts(query="c# <your pattern>", limit=5)
```
If a relevant skill/prompt is found → read it → apply its guidance.

**Step 3 — FALLBACK:**
If no skill found in Step 1 or Step 2 → proceed strictly with Master Document instructions.
**Do NOT invent patterns not described in this document.** If the document doesn't cover the case → STOP AND REPORT TO HUMAN.

**Scope — apply this policy for:**
- Writing a new script from scratch
- Choosing between two implementation approaches
- Deciding on a Unity-specific pattern (coroutine vs Update, event vs direct call)

**Skip this policy for:**
- Copying code already provided in §5 of this document
- Setting Inspector values, build settings, material properties
- Git/Beads/repomix operations

## GIT RULES

- НЕ делай git add, git commit, git push, git remote — всё это делает человек через GitHub Desktop
- НЕ создавай и не переключай ветки — это делает человек
- После каждой задачи предложи: commit title + description + имя ветки
- Формат commit title: `feat(scope): описание`
- Формат имени ветки: `feature/OE-MODULE-NNN-short-description`
- Remote репо проекта: `https://github.com/Dossne/Dima_D4.git` (только для контекста, не трогать)

## REPOMIX READING STRATEGY

Не читай snapshot целиком — экономь токены.

| Текущая задача | Что читать |
|---|---|
| OE-SETUP-001 | Только CHANGELOG.md — кода ещё нет |
| OE-CORE-001 — OE-INPUT-001 | `repomix --include "Assets/Scripts/**"` |
| OE-PLAYER-* — OE-STATE-* | `repomix --include "Assets/Scripts/Core/**,Assets/Scripts/<твой модуль>/**"` |
| OE-UI-* — OE-BUILD-001 | `repomix --include "Assets/Scripts/Core/**,Assets/Scripts/UI/**"` |

**Обновляй snapshot только:**
- После завершения задачи (перед CHANGELOG)
- После получения "checkpoint passed" от человека
- НЕ обновляй в середине задачи

`GameEvents.cs` всегда читай целиком — он маленький и критичный.
Если snapshot > 500 строк — читай только первые 200 строк + весь `Assets/Scripts/Core/`.
