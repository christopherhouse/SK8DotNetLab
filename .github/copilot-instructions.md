# GitHub Copilot Instructions for SK8DotNet Lab

## Project Overview
SK8DotNetLab is an AI chatbot solution consisting of a Blazor Server frontend and ASP.NET Core Web API backend designed to integrate with OpenAI for chat completions. This project demonstrates modern .NET development practices with cloud deployment capabilities.

## Solution Architecture

### Project Structure
```
SK8DotNetLab/
├── SK8DotNet.Chat/          # Blazor Server application (Frontend)
├── SK8DotNet.API/           # ASP.NET Core Web API (Backend)
├── SK8DotNet.API.Tests/     # XUnit test project
├── deploy/                  # Azure Container Apps deployment configs
└── docker-compose.yml       # Local development orchestration
```

### Component Responsibilities
- **SK8DotNet.Chat**: Blazor Server UI for chat interface, handles user interactions
- **SK8DotNet.API**: RESTful API backend, integrates with OpenAI for chat completions
- **SK8DotNet.API.Tests**: Comprehensive test suite using XUnit framework

## Development Guidelines

### .NET Requirements
- **MUST use .NET 8 runtime** for all projects
- Enable nullable reference types (`<Nullable>enable</Nullable>`)
- Use implicit usings where appropriate (`<ImplicitUsings>enable</ImplicitUsings>`)

### Testing Standards
- **Unit Testing Framework**: XUnit (already configured)
- **UI Testing Framework**: Playwright (approved for UI automation)
- **Test Coverage**: Aim for comprehensive test coverage of business logic
- **Test Organization**: Follow AAA pattern (Arrange, Act, Assert)
- Place unit tests in `SK8DotNet.API.Tests` project
- Create Playwright UI tests in separate test project when implementing UI testing

### NuGet Package Management
- **ONLY use General Availability (GA) packages** - no beta, prerelease, or alpha versions
- Prefer Microsoft packages for core functionality
- Keep package versions current but stable
- Document significant package additions in commit messages

### Code Quality Requirements
- **MUST validate all changes with quality tools** before committing
- Run `dotnet build` to ensure compilation success
- Execute `dotnet test` to verify all tests pass
- Follow established code patterns and conventions
- Maintain consistent naming conventions across projects

### AI Integration Context
- Primary integration target: OpenAI API for chat completions
- Implement proper error handling and retry logic for API calls
- Consider rate limiting and token management
- Store API keys and sensitive configuration in secure configuration (not hardcoded)
- Design for scalability and concurrent user sessions

### Architecture Adherence
When making changes, **ALWAYS**:
1. **Respect the separation of concerns** between Chat UI and API backend
2. **Maintain the existing project structure** - don't restructure without explicit requirements
3. **Follow established patterns** for dependency injection, configuration, and logging
4. **Preserve containerization compatibility** - ensure changes work with Docker setup
5. **Consider Azure Container Apps deployment** - validate environment variable usage

### Development Workflow
1. **Build First**: Always run `dotnet build` to verify compilation
2. **Test Early**: Run existing tests with `dotnet test` before making changes
3. **Incremental Changes**: Make small, focused commits with clear messages
4. **Validate Quality**: Use built-in analyzers and ensure no new warnings
5. **Document Impact**: Update relevant documentation for significant changes

### Configuration and Security
- Use ASP.NET Core configuration system (appsettings.json, environment variables)
- Implement proper logging with Application Insights integration
- Never commit secrets or API keys to source control
- Use dependency injection for service registration
- Configure CORS appropriately for API access

### Performance Considerations
- Implement async/await patterns for I/O operations
- Consider caching strategies for frequently accessed data
- Monitor Application Insights telemetry for performance insights
- Design API endpoints for efficient data transfer

### Additional Efficiency Guidelines
- **Leverage existing infrastructure**: Use configured Application Insights, Docker setup
- **Follow RESTful principles** for API design
- **Implement proper HTTP status codes** and error responses
- **Use built-in ASP.NET Core features** (model binding, validation, middleware)
- **Optimize for Azure Container Apps** deployment model
- **Consider SignalR** for real-time chat features if needed
- **Implement health checks** for monitoring and deployment validation

### UI Development (Blazor Server)
- Follow Blazor Server patterns and lifecycle management
- Implement proper component state management
- Use CSS classes and maintain consistent styling
- Consider accessibility requirements
- Optimize for server-side rendering performance

### Quality Validation Checklist
Before submitting any changes:
- [ ] Code compiles without warnings (`dotnet build`)
- [ ] All existing tests pass (`dotnet test`)
- [ ] New functionality includes appropriate tests
- [ ] Code follows established patterns and conventions
- [ ] No hardcoded secrets or configuration values
- [ ] Docker containerization still works (if applicable)
- [ ] Documentation updated for significant changes

## Examples of Preferred Patterns

### API Controller Pattern
```csharp
[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IChatService chatService, ILogger<ChatController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> SendMessage([FromBody] ChatRequest request)
    {
        // Implementation
    }
}
```

### Service Registration Pattern
```csharp
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();
```

### XUnit Test Pattern
```csharp
public class ChatServiceTests
{
    [Fact]
    public async Task SendMessage_WithValidInput_ReturnsResponse()
    {
        // Arrange
        var service = new ChatService(mockConfig, mockLogger);
        var request = new ChatRequest { Message = "Hello" };

        // Act
        var result = await service.SendMessageAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Response);
    }
}
```

Remember: **Quality first, consistency always, efficiency through proper patterns.**